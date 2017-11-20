using AdventureHelper.Website.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class DndJournalManager
    {
        //this probably isn't that great of an idea...
        private readonly SimpleFileBank<Document> DocumentRepository;
        private Document[] _docs;
        private Document[] AllDocuments
        {
            get
            {
                if (_docs == null || DocumentsAreDirty)
                {
                    _docs = DocumentRepository.Get();
                }
                return _docs;
            }
        }

        private bool DocumentsAreDirty;

        public DndJournalManager(SimpleFileBank<Document> documentRepository)
        {
            this.DocumentRepository = documentRepository;
            DocumentsAreDirty = false;
        }

        public JournalEntryDto[] GetJournalEntries(Guid userId)
        {
            return AllDocuments
                .Where(d => d.DocumentType == EntryMetaKeys.DocumentType)
                .Where(d => d.OwnerId == userId)
                .Select(d => DocToJournal(d))
                .ToArray();
        }

        internal CharacterDto GetOrCreateCharacter(string characterName)
        {
            var character = AllDocuments.FirstOrDefault(d => d.DocumentType == CharacterMetaKeys.DocumentType && d.Title.Equals(characterName, StringComparison.CurrentCultureIgnoreCase));
            if (character != null)
                return DocToCharacter(character);

            var newUserId = Guid.NewGuid();
            var newCharacter = new Document(
                title: characterName,
                body: "",
                dateCreated: DateTime.Now,
                dateLastModified: DateTime.Now,
                documentType: CharacterMetaKeys.DocumentType,
                id: newUserId,
                metaData: new Dictionary<string, string>(),
                ownerId: newUserId);

            SaveDocument(newCharacter);
            return DocToCharacter(newCharacter);
        }

        public JournalLinksDto[] GetJournalLinks(Guid userId)
        {
            return AllDocuments
                .Where(d => d.DocumentType == LinkMetaKeys.DocumentType)
                .Select(d => DocToLink(d))
                .Where(l => l.OwnerId == userId || l.Shared)
                .ToArray();
        }

        public JournalEntryDto SaveJournalEntry(JournalEntryDto entry, Guid userId)
        {
            if (entry.Id.HasValue)
            {
                var existingDocument = AllDocuments.Single(d => d.Id == entry.Id.Value);
                if (existingDocument.OwnerId != userId)
                {
                    throw new UnauthorizedAccessException("Cannot save someone else's entry");
                }

                existingDocument.Body = entry.Body;
                existingDocument.DateLastModified = DateTime.Now;
                existingDocument.Title = entry.Name;

                SaveDocument(existingDocument);
                return DocToJournal(existingDocument);
            }
            else
            {
                var newDocument = new Document(
                    body: entry.Body,
                    title: entry.Name,
                    documentType: EntryMetaKeys.DocumentType,
                    dateCreated: DateTime.Now,
                    dateLastModified: DateTime.Now,
                    id: Guid.NewGuid(),
                    metaData: new Dictionary<string, string>(),
                    ownerId: userId);
                SaveDocument(newDocument);
                return DocToJournal(newDocument);
            }
        }

        public JournalLinksDto SaveLink(JournalLinksDto link, Guid userId)
        {
            if (link.Id.HasValue)
            {
                var existingLink = AllDocuments.Single(d => d.Id == link.Id.Value);

                if (existingLink.OwnerId == userId)
                {
                    existingLink.MetaData[LinkMetaKeys.IsShared] = link.Shared.ToString();
                }
                existingLink.Title = link.Name;
                existingLink.MetaData[LinkMetaKeys.LinkType] = link.Type;
                existingLink.Body = link.Body;

                SaveDocument(existingLink);
                return DocToLink(existingLink);
            }
            else
            {
                var newLink = new Document(
                    id: Guid.NewGuid(),
                    ownerId: userId,
                    body: link.Body,
                    dateCreated: DateTime.Now,
                    dateLastModified: DateTime.Now,
                    documentType: LinkMetaKeys.DocumentType,
                    metaData: new Dictionary<string, string>()
                    {
                        { LinkMetaKeys.IsShared, link.Shared.ToString() },
                        { LinkMetaKeys.LinkType, link.Type }
                    },
                    title: link.Name);

                SaveDocument(newLink);
                return DocToLink(newLink);
            }
        }

        private void SaveDocument(Document doc)
        {
            DocumentsAreDirty = true;
            doc.DateLastModified = DateTime.Now;
            DocumentRepository.Save(doc);
        }

        private JournalEntryDto DocToJournal(Document doc)
        {
            return new JournalEntryDto(
                    body: doc.Body,
                    id: doc.Id,
                    name: doc.Title);
        }

        private JournalLinksDto DocToLink(Document doc)
        {
            return new JournalLinksDto(
                        body: doc.Body,
                        id: doc.Id,
                        name: doc.Title,
                        shared: bool.Parse(doc.MetaData[LinkMetaKeys.IsShared]),
                        type: doc.MetaData[LinkMetaKeys.LinkType],
                        ownerId: doc.OwnerId);
        }


        private CharacterDto DocToCharacter(Document doc)
        {
            return new CharacterDto(
                name: doc.Title,
                id: doc.Id);
        }

        //should probably make an attribute system for document -> dto conversion
        private static class CharacterMetaKeys
        {
            public const string DocumentType = "dnd-character";
        }

        private static class EntryMetaKeys
        {
            public const string DocumentType = "dnd-journal-entry";
        }

        private static class LinkMetaKeys
        {
            public const string DocumentType = "dnd-journal-link";
            public const string LinkType = "link-type";
            public const string IsShared = "is-shared";
        }
    }
}