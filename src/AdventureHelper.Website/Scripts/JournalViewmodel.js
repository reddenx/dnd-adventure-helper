/// <reference path="~/scripts/journalapi.js" />
/// <reference path="~/scripts/Autosize.js" />

var api = new JournalApi();
var vm = new Vue({
    el: '#bound-context',
    data: {
        entries: [],
        links: [],
        userId: '',
        asyncOperationsCount: 0,

        linkViewer: {
            show: false,
            position: { x: 0, y: 0 },
            name: 'linkviewer-name',
            type: 'linkviewer-type',
            body: 'linkviewer-body',
            contentList: [],
            linkData: {},
        },

        linkEditor: {
            show: false,
            asyncActions: 0,
            name: 'linkeditor-name',
            type: 'linkeditor-type',
            body: 'linkeditor',
            shared: false,
            position: { x: 0, y: 0, },
            savedData: {},
            parsedLinks: [],
        },
        linkList: {
            linkTypes: [],
            linkTypeTree: {},
        },
        editor: {
            show: false,
            asyncActions: 0,
            parsedLinks: [],
            body: '',
            name: '',
            savedData: {},
        },
        entryViewer: {
            show: false,
            viewedEntry: {},
            contentList: '',
        },
        entryListViewer: {
            show: true,
        },

        characterEntry: {
            busy: false,
            characterName: '',
        }
    },
    mounted: function () {
        this.showCharacterModal();
    },
    methods: {
        refreshEntries: function () {
            console.debug('refresh-entries');
            this.asyncOperationsCount += 1;

            var self = this;
            api.getEntries(this.userId, function (entries) {
                self.entries = [];
                self.entries = entries;
                self.asyncOperationsCount -= 1;
                self.entryListRefresh();
            });
        },
        refreshLinks: function () {
            console.debug('refresh-links');
            this.asyncOperationsCount += 1;

            var self = this;
            api.getLinks(this.userId, function (links) {
                self.links = links;
                self.asyncOperationsCount -= 1;
                self.linkListRefresh();
            });
        },
        getLinkedContentFromText: function (text) {
            var contentList = [];
            if (!text) {
                return contentList;
            }
            var self = this;
            text.split(entryReferenceRegex).forEach(function (item, index) {
                if (index % 2 != 0) {
                    var foundLink = _.find(self.links, function (link) {
                        return link.name == item;
                    });
                    if (foundLink) {
                        contentList.push(foundLink);
                    } else {
                        contentList.push(new Link(item));
                    }


                } else if (item && item.length > 0) {
                    contentList.push(item);
                }
            });
            return contentList;
        },

        //character entry
        showCharacterModal: function () {
            console.debug('show-character-modal');

            $('#character-modal').modal({
                show: true,
                backdrop: 'static',
                keyboard: false,
            });
        },
        loadFromModal: function () {
            console.debug('load-from-modal');

            if (this.characterEntry.characterName.length <= 0)
                return;

            this.characterEntry.busy = true;
            var self = this;
            api.getCharacter(this.characterEntry.characterName, function (character) {
                self.userId = character.id;
                self.refreshEntries();
                self.refreshLinks();
                self.characterEntry.busy = false;
                $('#character-modal').modal('hide');
            });
        },

        //entry list viewer
        entryListRefresh: function () {
            console.debug('entry-list-refresh');
        },
        showEntryList: function () {
            console.debug('show-entry-list');

            this.entryViewer.show = false;
            this.editor.show = false;
            this.entryListViewer.show = true;
        },

        //entry viewer
        viewerLoad: function (entry) {
            console.debug('viewer-load');
            this.entryViewer.viewedEntry = entry;

            this.entryViewer.contentList = this.getLinkedContentFromText(entry.body);

            this.showViewer();
        },
        viewerClose: function () {
            console.debug('viewer-close');

            this.showEntryList();
        },
        showViewer: function () {
            console.debug('show-viewer');

            this.entryViewer.show = true;
            this.editor.show = false;
            this.entryListViewer.show = false;
        },

        //entry editor
        editorSave: function () {
            console.debug('editor-save');

            var newEntry = new JournalEntry(this.editor.name, this.editor.body, this.editor.savedData.id);
            this.editor.asyncActions++;
            var self = this;
            api.saveEntry(this.userId, newEntry, function () {
                self.editor.asyncActions--;
                self.refreshEntries();
            });

            this.showEntryList();
        },
        editorCancel: function () {
            console.debug('editor-cancel');

            this.editorResetToData();

            this.showEntryList();
        },
        editorLoad: function (entry) {
            console.debug('editor-load');
            this.editor.savedData = entry;
            this.editorResetToData();

            this.showEditor();
        },
        editorResetToData: function () {
            console.debug('editor-reset-data');
            this.editor.name = this.editor.savedData.name;
            this.editor.body = this.editor.savedData.body;
            this.editorBodyChanged();
        },
        editorBodyChanged: _.debounce(function () {
            console.debug('editor-body-changed');

            var linkNames = parseTextForLinks(this.editor.body);

            this.editor.parsedLinks = [];
            var self = this;
            linkNames.forEach(function (linkName) {
                var foundLink = _.find(self.links, function (link) { return link.name == linkName; });
                if (foundLink) {
                    self.editor.parsedLinks.push(foundLink);
                } else {
                    self.editor.parsedLinks.push(new Link(linkName));
                }
            });
        }, 1200),
        showEditor: function () {
            console.debug('show-editor');

            this.entryViewer.show = false;
            this.editor.show = true;
            this.entryListViewer.show = false;
        },

        startNewEntry: function () {
            console.debug('start-new-entry');

            this.editorLoad({});
        },

        //link list
        linkListRefresh: function () {
            console.debug('link-list-refresh');

            this.linkList.linkTypes = _.uniq(this.links.map(function (link) { return link.type; }));
            this.linkList.linkTypeTree = {};
            var self = this;
            this.linkList.linkTypes.forEach(function (type) {
                self.linkList.linkTypeTree[type] = [];
            });
            this.links.forEach(function (link) {
                self.linkList.linkTypeTree[link.type].push(link);
            });
        },

        //link viewer
        linkViewerLoad: function (link, x, y) {
            console.debug('link-viewer-load');

            this.linkViewer.linkData = link;
            this.linkViewer.name = link.name;
            this.linkViewer.type = link.type;
            this.linkViewer.body = link.body;
            this.linkViewer.shared = link.shared;

            this.linkViewer.contentList = [];
            this.linkViewer.contentList = this.getLinkedContentFromText(link.body);

            this.linkViewer.show = true;

            if (x || y) {
                this.linkViewer.position.x = x;
                this.linkViewer.position.y = y;
            }
        },
        linkViewerClose: function () {
            console.debug('link-viewer-close');

            this.linkViewer.show = false;
        },

        //link editor
        linkEditorLoad: function (entry, x, y) {
            console.debug('link-editor-load');
            this.linkEditor.savedData = entry;
            this.linkEditorResetToData();

            this.linkEditor.show = true;
            this.linkViewer.show = false;

            if (x || y) {
                this.linkEditor.position.x = x;
                this.linkEditor.position.y = y;
            }
        },
        linkEditorResetToData: function () {
            console.debug('link-editor-reset-data');
            this.linkEditor.name = this.linkEditor.savedData.name;
            this.linkEditor.type = this.linkEditor.savedData.type;
            this.linkEditor.body = this.linkEditor.savedData.body;
            this.linkEditor.shared = this.linkEditor.savedData.shared;
        },
        linkEditorSave: function () {
            console.debug('link-editor-save');

            var newLink = new Link(this.linkEditor.name, this.linkEditor.type, this.linkEditor.body, this.linkEditor.savedData.id, this.linkEditor.shared);

            this.linkEditor.asyncActions++;
            var self = this;
            api.saveLink(this.userId, newLink, function () {
                console.debug('link-editor-save-sucess');

                self.linkEditor.asyncActions--;
                self.refreshLinks();
                self.linkEditor.show = false;
            });
        },
        linkEditorCancel: function () {
            console.debug('link-editor-cancel');
            this.linkEditorResetToData();
            this.linkEditor.show = false;
        },

        startNewLink: function (x, y) {
            console.debug('start-new-link');
            this.linkEditorLoad({}, x, y);
        },

        getLinkButtonClass: function (type) {
            var linkTypeIndex = this.linkList.linkTypes.indexOf(type);
            if (linkTypeIndex < 0)
                return 'btn-secondary';

            //lets do some known ones
            var knownTypes = {
                'quest(open)': 'btn-danger',
                'quest(closed)': 'btn-outline-danger',
                'location': 'btn-success',
                'character': 'btn-info'
            }
            if (knownTypes[type]) {
                return knownTypes[type];
            }




            var hash = 0, i, chr;
            //if (type.length === 0) return hash;
            for (i = 0; i < type.length; i++) {
                chr = type.charCodeAt(i);
                hash = ((hash << 5) - hash) + chr;
                hash |= 0; 
            }

            var buttonClasses = [
                'btn-primary',
                //'btn-secondary',
                'btn-success',
                'btn-danger',
                'btn-warning',
                'btn-info',
                'btn-dark',

                'btn-outline-primary',
                'btn-outline-secondary',
                'btn-outline-success',
                'btn-outline-danger',
                'btn-outline-info',
                'btn-outline-dark',
            ];

            return buttonClasses[Math.abs(hash) % buttonClasses.length];


            var colors = [{"background":"#bfd4f2","text":"#000000"},{"background":"#d93f0b","text":"#ffffff"},{"background":"#1d76db","text":"#ffffff"},{"background":"#0052cc","text":"#ffffff"},{"background":"#f9d0c4","text":"#000000"},{"background":"#bfdadc","text":"#000000"},{"background":"#5319e7","text":"#ffffff"},{"background":"#c5def5","text":"#000000"},{"background":"#b60205","text":"#ffffff"},{"background":"#fef2c0","text":"#000000"},{"background":"#0e8a16","text":"#ffffff"},{"background":"#e99695","text":"#000000"},{"background":"#c2e0c6","text":"#000000"},{"background":"#fbca04","text":"#ffffff"},{"background":"#d4c5f9","text":"#000000"},{"background":"#006b75","text":"#ffffff"}]

            var colors = [
                { background: '#b60205', text: '#ffffff' },
                { background: '#d93f0b', text: '#ffffff' },
                { background: '#fbca04', text: '#ffffff' },
                { background: '#0e8a16', text: '#ffffff' },
                { background: '#006b75', text: '#ffffff' },
                { background: '#1d76db', text: '#ffffff' },
                { background: '#0052cc', text: '#ffffff' },
                { background: '#5319e7', text: '#ffffff' },
                { background: '#e99695', text: '#000000' },
                { background: '#f9d0c4', text: '#000000' },
                { background: '#fef2c0', text: '#000000' },
                { background: '#c2e0c6', text: '#000000' },
                { background: '#bfdadc', text: '#000000' },
                { background: '#c5def5', text: '#000000' },
                { background: '#bfd4f2', text: '#000000' },
                { background: '#d4c5f9', text: '#000000' }];

            var color = colors[linkTypeIndex % colors.length];
            return 'background-color: ' + color.background + ';color: ' + color.text + ';';
        },
    }
});

