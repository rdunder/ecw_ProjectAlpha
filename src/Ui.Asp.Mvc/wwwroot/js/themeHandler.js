




document.addEventListener("DOMContentLoaded", async () => {
    if (document.documentElement.classList.contains("theme-dark")) {
        document.querySelector("#theme-toggle-checkbox").checked = true;
    }

    
})


async function ToggleTheme(toggleCheckBox) {
    const consentGiven = await checkIfConsentIsGiven("functional")
    const isThemeDark = toggleCheckBox.checked

    document.documentElement.classList.toggle("theme-dark")
    toggleCheckBox.checked = isThemeDark

    if (consentGiven) {
        localStorage.setItem("theme-dark", isThemeDark)
    }
}

