﻿var Character = function (name, id) {
    var self = this;

    self.name = name;
    self.id = id;

    return self;
}

var JournalEntry = function (name, body, id) {
    var self = this;

    self.name = name;
    self.body = body;
    self.id = id;

    return self;
}

var Link = function (name, type, body, id, shared, ownerId) {
    var self = this;

    self.name = name;
    self.type = type;
    self.body = body;
    self.shared = shared;
    self.id = id;
    self.ownerId = ownerId;

    return self;
}

var JournalApi = function () {
    var self = this;
    var m = {};

    self.getCharacter = function (characterName, callback) {
        $.ajax({
            url: '/journal/api/characters/' + characterName,
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                callback(new Character(data.Name, data.Id));
            },
        });
    }

    self.getLinks = function (userId, callback) {
        $.ajax({
            url: '/journal/api/links/' + userId,
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                callback(data.map(function (item) {
                    return new Link(item.Name, item.Type, item.Body, item.Id, item.Shared, item.OwnerId);
                }));
            }
        })
    }

    self.saveLink = function (userId, link, callback) {
        $.ajax({
            url: '/journal/api/links/' + userId,
            type: 'POST',
            data: JSON.stringify({
                data: {
                    Id: link.id,
                    Name: link.name,
                    Type: link.type,
                    Body: link.body,
                    Shared: link.shared,
                    OwnerId: userId,
                }
            }),
            dataType: 'json',
            contentType: 'application/json',
            complete: function () { callback(); }
        })
    }

    self.getEntries = function (userId, callback) {
        $.ajax({
            url: '/journal/api/entries/' + userId,
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

    self.saveEntry = function (userId, entry, callback) {
        $.ajax({
            url: '/journal/api/entries/' + userId,
            type: 'POST',
            data: JSON.stringify({
                data: {
                    Id: entry.id,
                    Name: entry.name,
                    Body: entry.body,
                    OwnerId: userId,
                }
            }),
            dataType: 'json',
            contentType: 'application/Json',
            complete: function (data) {
                callback();
            }
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
        return item[1];//.replace('-', ' ');
    }).filter(function (item) {
        return hashMatches.hasOwnProperty(item) ? false : (hashMatches[item] = true);
    });
    return filter;
}