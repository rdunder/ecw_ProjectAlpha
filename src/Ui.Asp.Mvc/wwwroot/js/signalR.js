const signalRConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build()

signalRConnection.on("ReceiveNotification", function (notification) {

    const container = document.querySelector("#notificationsContainer")

    const item = document.createElement('div')
    item.classList.add("notification-item")
    item.setAttribute('data-id', notification.id)
    item.innerHTML =
        `
        <img class="icon" src="${notification.icon}" />
        <div class="content">
            <div class="message">${notification.message}</div>
            <div class="time" data-created="${new Date(notification.created).toISOString()}">${notification.created}</div>
        </div>
        <button class="btn-close" onclick="dismissNotification('${notification.id}')"></button>
        `

    container.insertBefore(item, container.firstChild)
    updateRelativeTimes()
    updateNotificationCount()
})

signalRConnection.on("NotificationDismissed", function (notificationId) {
    removeNotification(notificationId)
})

signalRConnection.start().catch(err => console.error(`SignalR failed to start: ${err}`))

function updateRelativeTimes() {
    const now = new Date()
    const timeElements = document.querySelectorAll('.notification-item .time')

    timeElements.forEach(lmnt => {
        const created = new Date(lmnt.getAttribute('data-created'))
        const diff = now - created

        const seconds = Math.floor(diff / 1000)
        const minutes = Math.floor(seconds / 60)
        const hours = Math.floor(minutes / 60)
        const days = Math.floor(hours / 24)
        const weeks = Math.floor(days / 7)

        if      (weeks > 0) lmnt.textContent = `${weeks} week${weeks > 1 ? 's' : ''} ago`
        else if (days > 0) lmnt.textContent = `${days} day${days > 1 ? 's' : ''} ago`
        else if (hours > 0) lmnt.textContent = `${hours} hour${hours > 1 ? 's' : ''} ago`
        else if (minutes > 0) lmnt.textContent = `${minutes} minute${minutes > 1 ? 's' : ''} ago`
        else        lmnt.textContent = `${seconds} second${seconds !== 1 ? 's' : ''} ago`
    })
}

function updateNotificationCount() {
    const container = document.querySelector("#notificationsContainer")
    const counter = document.querySelector('.notification-number')
    const dropDownBtn = document.querySelector('#notificationDropdown')

    let count = container.querySelectorAll('.notification-item').length

    if (counter) {
        counter.textContent = count
    }

    const dot = dropDownBtn.querySelector('.dot.dot-red')
    if (count > 0 && !dot) {        
        const dot = document.createElement('div')
        dot.className = 'dot dot-red'
        dropDownBtn.appendChild(dot)
    }

    if (count === 0 && dot) {
        console.log("there is 0 notifications")
        dot.remove()
    }
}

function removeNotification(id) {
    const item = document.querySelector(`.notification-item[data-id="${id}"]`)
    console.log(id)
    if (!item) return
    item.remove()
    updateNotificationCount()
}

async function dismissNotification(notificationId) {
    try {
        const res = await fetch(`/api/notifications/dismiss/${notificationId}`, {
            method: 'POST'
        })

        if (res) {
            removeNotification(notificationId)
        } else {
            console.error(`Error when removing cotification: <Response = False>`)
        }
    } catch (err) {
        console.error(`Error when removing cotification: ${err}`)
    }
}


