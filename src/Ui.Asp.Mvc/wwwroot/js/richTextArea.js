


function isDarkMode() {
    return document.body.classList.contains('theme-dark');
}


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


    //  Contents of oxide-dark is copied over to custom-skin/skin.min.css
    //  i  ahve then edited the css to match the design to create a Dark Theme
    skin: 'custom-skin',
    skin_url: '/css/tiny-mce/custom-skin',


    

    init_instance_callback: function (editor) {
        const container = editor.getContainer();
        container.style.visibility = 'visible';
    },
        

    setup: function (editor) {
        editor.on('change', function () {
            editor.save();
        });
    },

});


