// Method to create unique scope for our JS implementation.
var registerNamespace = Function.registerNamespace = function (namespacePath) {
    var rootObject = window;
    var namespaceParts = namespacePath.split('.');
    var iCount = namespaceParts.length;
    for (var i = 0; i < iCount; i++) {
        var currentPart = namespaceParts[i];
        if (!rootObject[currentPart]) {
            rootObject[currentPart] = {};
        }
        rootObject = rootObject[currentPart];
    }
};

registerNamespace("YoeJoy");