﻿document.addEventListener("DOMContentLoaded", async () => {
    const consentBanner = document.querySelector("#cookieConsent")
    const consentAllbtn = document.querySelector("#cookieConsent button[data-cookie-string]")
    

    if (!await checkIfConsentIsGiven("general")) {
        consentBanner.classList.toggle("show")
    }

    consentAllbtn.addEventListener("click", function (e) {
        document.cookie = consentAllbtn.dataset.cookieString;
        SetCookiePrefs()
        consentBanner.classList.toggle("show")
    }, false)

    //  This eventlistener is moved to the ShowCookieBtn at _CookieConsent.cshtml
    //document.querySelector("#showCookieConsentBtn")
    //    .addEventListener("click", () => {
    //        consentBanner.classList.toggle("show")
    //    })

    
    getCookieConsentPrefs()
})

function getCookieConsentPrefs() {
    fetch('/api/CookieConsent')
        .then(res => res.json())
        .then(data => {
            document.querySelector("#cookieFunctional").checked = data.functional
        })
}

function SetCookiePrefs() {
    const prefs = {
        functional: document.querySelector("#cookieFunctional").checked 
    }

    fetch('/api/CookieConsent', {
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(prefs)
    })

    removeConsentStorage()
}

function toggleCookieConsentBanner() {
    document.querySelector("#cookieConsent").classList.toggle("show")
}

async function checkIfConsentIsGiven(category) {
    const res = await fetch(`/api/CookieConsent/IsConsentGiven?category=${category}`)
    return res.json()
}

