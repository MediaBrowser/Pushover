define(['globalize', 'pluginManager', 'emby-input'], function (globalize, pluginManager) {
    'use strict';

    function EntryEditor() {
    }

    EntryEditor.setObjectValues = function (context, entry) {

        entry.FriendlyName = context.querySelector('.txtFriendlyName').value;
        entry.Options.Token = context.querySelector('.txtToken').value;
        entry.Options.UserKey = context.querySelector('.txtUserKey').value;
        entry.Options.DeviceName = context.querySelector('.txtDeviceName').value;
        entry.Options.Priority = context.querySelector('.selectPriority').value;
    };

    EntryEditor.setFormValues = function (context, entry) {

        context.querySelector('.txtFriendlyName').value = entry.FriendlyName || '';
        context.querySelector('.txtToken').value = entry.Options.Token || '';
        context.querySelector('.txtUserKey').value = entry.Options.UserKey || '';
        context.querySelector('.txtDeviceName').value = entry.Options.DeviceName || '';
        context.querySelector('.selectPriority').value = entry.Options.Priority || '0';
    };

    EntryEditor.loadTemplate = function (context) {

        return require(['text!' + pluginManager.getConfigurationResourceUrl('pushovereditortemplate')]).then(function (responses) {

            var template = responses[0];
            context.innerHTML = globalize.translateDocument(template);

            // setup any required event handlers here
        });
    };

    EntryEditor.destroy = function () {

    };

    return EntryEditor;
});