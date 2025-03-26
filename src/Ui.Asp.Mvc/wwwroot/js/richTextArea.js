tinymce.init({
    selector: '#richTextArea',
    plugins: 'lists link autolink',

    branding: false,
    statusbar: false,
    toolbar: 'bold italic underline | alignleft aligncenter alignright | bullist numlist | link',
    toolbar_location: 'bottom',
    menubar: false,
    height: 200,
    resize: false,

    //  Cant find a way to load the css file, so cant style this.
    content_css: ['/css/site.css'],
    body_class: 'rich-text-are-body',
    content_style: `
        body {
        }
    `,

    setup: function (editor) {
        editor.on('change', function () {
            editor.save();
        });
    },

});