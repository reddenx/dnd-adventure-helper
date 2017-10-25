var JournalEntry = function (name, body, metaData) {
    var self = this;

    self.name = name;
    self.body = body;
    self.metaData = metaData || {};

    self.ToDocumentDto = function () {
        return new DocumentDto(self.name, {
            'document-type': 'journal-entry'
        }, self.body);
    }

    return self;
}

var Link = function (name) {
    var self = this;

    self.name = name;

    return self;
}

var DocumentDto = function (name, metaData, body) {
    this.Name = name,
    this.MetaData = metaData,
    this.Body = body;

    return this;
}

var AdventureJournal = function () {
    var self = this;

    self.journalEntries = [];

    self.characters = [];
    self.items = [];
    self.locations = [];
    self.other = [];

    self.RefreshJournalData = function () {
        $.ajax({
            url: '/journal/json/documents',
            method: 'GET',
            success: function (data) {
                self.HandleJournalData(data);
            },
            error: function () {
            }
        });
    }

    self.HandleJournalData = function (data) {
        self.journalEntries = data.filter(function (item) {
            return item.MetaData && item.MetaData['document-type'] === 'journal-entry';
        }).map(function (item) {
            return new JournalEntry(item.Name, item.Body, item.MetaData);
        });
    }

    self.SaveDocument = function (document) {
        $.ajax({
            url: '/journal/json/documents',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ document: document }),
            success: function () {
                self.RefreshJournalData();
            }
        });
    }

    self.SaveLink = function (link) {
        var dto = self.GetLinkDto(link);
        self.SaveDocument(new DocumentDto(link.name,
            {
                type: link.type,
            }, link.description));
    }

    self.RefreshJournalData();

    return self;
}

var entryReferenceRegex = /#([\w-_]+)/g;
var ParseTextForLinks = function (text) {

    var matches = [];
    var match;
    while (match = entryReferenceRegex.exec(text)) {
        matches.push(match);
    }

    var hashMatches = {};
    return matches.map(function (item) {
        return item[1].replace('-', ' ');
    }).filter(function (item) {
        return hashMatches.hasOwnProperty(item) ? false : (hashMatches[item] = true);
    });
    return filter;

}