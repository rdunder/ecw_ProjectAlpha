const signalRConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build()

signalRConnection.on("AllReceiveNotification", function (notification) {
    const container = document.querySelector("#notificationsContainer")

    const item = document.createElement('div')
    item.classList.add("notification-item")
    item.setAttribute('data-id', notification.id)
    item.innerHTML =
        `
        <img class="icon" src="${notification.type.icon}" />
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
        dot.remove()
    }
}

function removeNotification(id) {
    const item = document.querySelector(`.notification-item[data-id="${id}"]`)
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