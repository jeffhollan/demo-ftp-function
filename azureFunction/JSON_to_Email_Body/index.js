module.exports = function (context, data) {
    context.log('Webhook was triggered!');

    context.res = {
        body: `<body><p>Hello ${data.Name}<br/>Thank you for signing up.  If you have any questions feel free to reach out.<br/>${data.AssignedContact}</p></body>`
    }

    context.done();
}
