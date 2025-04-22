document.addEventListener("DOMContentLoaded", () => {

    //  Get current cookie consent preferences
    const cookieConsentPrefs = getCookieConsentPrefs()

})

async function getCookieConsentPrefs() {
    fetch('/api/CookieConsent')
        .then(res => res.json())
        .then(data => console.log(data))
}

