

document.addEventListener("DOMContentLoaded", async () => {
    //  Uncomment these lines if you want to check for duplicate Id's in HTML
    //const resultDuplicateIds = findDuplicateIds();
    //console.log(resultDuplicateIds);

    //  Handle image previews
    document.querySelectorAll(".image-preview-container").forEach(pc => {
        const fileInput = pc.querySelector("input[type='file']");
        const circle = pc.querySelector("#circle-container");
        const img = pc.querySelector("#image-preview");


        pc.addEventListener('click', () => {
            fileInput.click();
        });

        fileInput.addEventListener("change", (e) => {
            const file = e.target.files[0];

            img.src = '';

            if (file) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    img.src = e.target.result;
                    img.classList.remove("d-none");
                    circle.classList.add("d-none");
                };

                reader.readAsDataURL(file);
            } else {
                img.classList.add("d-none");
                circle.classList.remove("d-none");
            }
        });
    });


    //  Handle submit forms from modals
    const modals = document.querySelectorAll('.modal')
    modals.forEach(modal => {        
        if (modal.classList.contains("not-validate")) return
        const form = modal.querySelector('form')

        
        
        //  Handle EndDate not being before StartDate
        const formStartDate = form.querySelector("input[type='date'][name='StartDate']")
        const formEndDate = form.querySelector("input[type='date'][name='EndDate']")
        
        if (formStartDate && formEndDate) {
            formStartDate.addEventListener("change", (e) => {
                if (e.target.value > formEndDate.value) {
                   formEndDate.value = e.target.value
                }
            })
        }

        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            form.querySelectorAll('[data-val="true"]').forEach(input => {
                input.classList.remove('input-validation-error')
            })

            form.querySelectorAll('[data-valmsg-for]').forEach(span => {
                span.innerText = ''
                span.classList.remove('field.validation-error')
            })            

            const formData = new FormData(form)

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })

                if (!res.ok) {
                    const data = await res.json()

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (span) {
                                span.innerText = data.errors[key].join('\n')
                                span.classList.add('field-validation-error')
                            }
                        })
                    }
                } else {
                    window.location.reload();
                }


            } catch {
                console.log("error when submitting the form")
            }
        })
    })
    
});

async function removeConsentStorage() {
    const functionalConsentGiven = await checkIfConsentIsGiven("functional")
    const analyticConsentGiven = await checkIfConsentIsGiven("analytics")
    const marketingConsentGiven = await checkIfConsentIsGiven("marketing")

    if (functionalConsentGiven) {
        localStorage.removeItem("theme-dark")
    }

    if (analyticConsentGiven) {

    }

    if (marketingConsentGiven) {

    }
}

function quillJsInit(editorId, toolbarId, content, textAreaId) {
    const textArea = document.querySelector(textAreaId);
    const quill = new Quill(editorId, {
        modules: {
            toolbar: toolbarId
        },
        placeholder: 'Enter Description',
        theme: 'snow'
    })

    if (content)
        quill.root.innerHTML = content

    quill.on('text-change', () => {
        textArea.value = quill.root.innerHTML;
    })
}