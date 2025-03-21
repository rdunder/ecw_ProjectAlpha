
document.addEventListener("DOMContentLoaded", () => {

    //  handle Form Image Previews
    document.querySelectorAll(".image-preview-container").forEach(pc => {
        const fileInput = pc.querySelector("input[type='file']")
        const circle = document.querySelector("#circle-container")

        pc.addEventListener('click', () => fileInput.click());

        fileInput.addEventListener("change", (e) => {
            const file = e.target.files[0];
            const img = document.querySelector("#image-preview");

            if (file) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    img.src = e.target.result
                    img.classList.remove("d-none")
                    circle.classList.add("d-none")
                }

                reader.readAsDataURL(file)
            } else {
                img.classList.add("d-none")
                circle.classList.remove("d-none")
            }

        })
    })



    //  Handle submit forms from modals
    const modals = document.querySelectorAll('.modal')

    modals.forEach(modal => {

        const form = modal.querySelector('form')

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


//  Dark Mode Toggle
function ToggleTheme() {
    document.body.classList.toggle("theme-dark"); 
}