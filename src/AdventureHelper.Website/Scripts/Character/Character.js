var Character = function (name, attributes) {
    var self = this;

    self.name = name || '';
    self.attributes = attributes || self.generateAttributes();

    self.generateAttributes = function () {
        self.attributes = [];
        self.attributes.push(new Attribute('strength', ['attribute']));
        self.attributes.push(new Attribute('dexterity', ['attribute']));
        self.attributes.push(new Attribute('constitution', ['attribute']));
        self.attributes.push(new Attribute('intelligence', ['attribute']));
        self.attributes.push(new Attribute('wisdom', ['attribute']));
        self.attributes.push(new Attribute('charisma', ['attribute']));

        
    }

    return self;
}

var Attribute = function (name, tags) {
    var self = this;

    self.name = name || '';
    self.tags = tags || [];

    return self;
}