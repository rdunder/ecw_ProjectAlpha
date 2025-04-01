


const quill = new Quill('#quill-editor', {
    modules: {
        toolbar:
        [
            ['bold', 'italic', 'underline'],
            ['align', { 'align': 'center' }, { 'align': 'right' }],
            [{ 'list': 'ordered' }, { 'list': 'bullet' }],
            ['link']
        ]
    },

    placeholder: 'Enter Descritption',
    theme: 'snow'
});


quill.on('text-change', (delta, oldDelta, source) => {
    if (source == 'api') {
        console.log('An API call triggered this change.');
    } else if (source == 'user') {
        console.log('A user action triggered this change.');
        console.log(quill.getSemanticHTML())
        document.querySelector('#description-input').value = quill.getSemanticHTML()
    }
});




//quill.on('editor-change', (e, args) => {
//    document.querySelector('#description-input').value = quill.getSemanticHTML()

//    console.log(quill.getSemanticHTML())
//    console.log(quill.getContents())
//})

