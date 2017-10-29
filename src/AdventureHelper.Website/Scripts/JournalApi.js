var JournalEntry = function (name, body, id) {
    var self = this;

    self.name = name;
    self.body = body;
    self.id = id;

    self.getParsedLinkNames = function () {
        return parseTextForLinks(self.body);
    }

    return self;
}

var Link = function (name, type, body, id) {
    var self = this;

    self.name = name;
    self.type = type;
    self.body = body;
    self.id = id;

    self.getParsedLinkNames = function () {
        return parseTextForLinks(self.body);
    }

    return self;
}

var JournalApi = function () {
    var self = this;
    var m = {};

    self.getLinks = function (callback) {
        $.ajax({
            url: 'api/links',
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                callback(data.map(function (item) {
                    return new Link(item.Name, item.Type, item.Body, item.Id);
                }));
            }
        })
    }

    self.saveLink = function (link) {
        $.ajax({
            url: 'api/links',
            type: 'POST',
            data: JSON.stringify({
                data: {
                    Name: link.name,
                    Type: link.type,
                    Body: link.body,
                    Id: link.id
                }
            }),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    self.getEntries = function (callback) {
        $.ajax({
            url: 'api/entries',
            type: 'GET',
            dataType: 'json',
            contentType: 'application/Json',
            success: function (data) {
                callback(data.map(function (item) {
                    return new JournalEntry(item.Name, item.Body, item.Id);
                }))
            }
        })
    }

    self.saveEntry = function (entry) {
        $.ajax({
            url: 'api/entries',
            type: 'POST',
            data: JSON.stringify({
                data: {
                    Name: entry.name,
                    Body: entry.body,
                    Id: entry.id,
                }
            }),
            dataType: 'json',
            contentType: 'application/Json',
        })
    }

    return self;
}

var entryReferenceRegex = /#([\w-_]+)/g;
var parseTextForLinks = function (text) {

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