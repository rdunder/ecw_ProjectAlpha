﻿//#region || SignalR Notification Hub
const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build()

notificationConnection.on("ReceiveNotification", function (notification) {
    

    const container = document.querySelector("#notificationsContainer")

    const item = document.createElement('div')
    item.classList.add("notification-item")
    item.setAttribute('data-id', notification.id)
    item.innerHTML =
        `
        <img class="icon" src="${notification.icon}" />
        <div class="content">
            <div class="type">${notification.typeName}</div>
            <div class="message">${notification.message}</div>
            <div class="time" data-created="${new Date(notification.created).toISOString()}">${notification.created}</div>
        </div>
        <button class="btn-close" onclick="dismissNotification('${notification.id}')"></button>
        `

    container.insertBefore(item, container.firstChild)
    updateRelativeTimes()
    updateNotificationCount()
})

notificationConnection.on("NotificationDismissed", function (notificationId) {
    removeNotification(notificationId)
})

notificationConnection.start().catch(err => console.error(`notificationHUB SignalR failed to start: ${err}`))

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

//#endregion

//#region || SignalR Presence Hub
const presenceSignalRConnection = new signalR.HubConnectionBuilder()
    .withUrl("/presenceHub")
    .build()

const currentOnlineUsers = new Set()

presenceSignalRConnection.on("UserConnected", (userId) => {
    currentOnlineUsers.add(userId)
    updateOnlineUsersFeedback(userId, true)
})

presenceSignalRConnection.on("UserDisconnected", (userId) => {
    currentOnlineUsers.delete(userId)
    updateOnlineUsersFeedback(userId, false);

})

presenceSignalRConnection.on("OnlineUsers", (users) => {
    users.forEach(id => {
        currentOnlineUsers.add(id)
        updateOnlineUsersFeedback(id, true)
    })    
})

presenceSignalRConnection.start().catch(err => console.error(`presenceHUB SignalR failed to start: ${err}`))


function updateOnlineUsersFeedback(userId, isOnline) {
    const userCard = document.querySelector(`.member-card[data-user-id="${userId}"]`)
    const presenceDot = userCard.querySelector(".presence-status")

    const presenceCount = document.querySelector('.presence-status-count')
    presenceCount.textContent = `Online: ${currentOnlineUsers.size}`

    if (isOnline) {
        presenceDot.classList.remove('presence-offline')
        presenceDot.classList.add('presence-online')
    } else {
        presenceDot.classList.remove('presence-online')
        presenceDot.classList.add('presence-offline')
    }
}
//#endregion

//#region || SignalR Message Hub
//  This implementaion is not complete, the messages are not stored in DB
//  therefore they dissapear on reload.
const messageSignalRConnection = new signalR.HubConnectionBuilder()
    .withUrl("/messageHub")
    .build()

messageSignalRConnection.on("RecieveMessage", (senderName, senderTitle, message, id) => {
    console.log(`Private message from ${senderName} [Title: ${senderTitle}] [ID: ${id}]:\n${message}`)

    const container = document.querySelector("#notificationsContainer")

    const item = document.createElement('div')
    item.classList.add("notification-item")
    item.setAttribute('data-id', id)
    item.innerHTML =
        `
        <img class="icon" src="/images/chat.svg" />
        <div class="content">
            <div class="type">${senderName} [${senderTitle}]</div>
            <div class="message">${message}</div>
            <div class="time" data-created="${new Date().toISOString()}">Created</div>
        </div>
        <button class="btn-close" onclick="dismissNotification('${id}')"></button>
        `

    container.insertBefore(item, container.firstChild)
    updateRelativeTimes()
    updateNotificationCount()


    //  Modal popup with message sent, a bit intrucive

    //const messageModal = `
    //<div class="modal fade" id="signalRMessageModal" tabindex="-1" aria-labelledby="signalRMessageModal" aria-hidden="true">
    //  <div class="modal-dialog">
    //    <div class="modal-content">
    //      <div class="modal-header">
    //        <h5 class="modal-title" id="dynamicModalLabel"><i class="bi bi-chat-fill"></i></h5>
    //        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
    //      </div>
    //      <div class="modal-body">
    //        <h6>Message from: ${senderName} [${senderTitle}]</h6>
    //        <p>${message}</p>
    //      </div>
    //      <div class="modal-footer">
    //        <button type="button" class="btn btn-blue" data-bs-dismiss="modal">Close</button>
    //      </div>
    //    </div>
    //  </div>
    //</div>
    //`

    //document.body.insertAdjacentHTML('beforeend', messageModal)
    //const modalEllement = document.querySelector("#signalRMessageModal")
    //const modal = new bootstrap.Modal(modalEllement)

    //modalEllement.addEventListener("hidden.bs.modal", () => {
    //    modal.dispose()
    //    modalEllement.remove()
    //})

    //modal.show()
})

messageSignalRConnection.start().catch(err => console.error(`messageHub SignalR failed to start: ${err}`))


function signalRSendMessage(userId, message) {
    messageSignalRConnection.invoke("SendMessage", userId, message)
}
//#endregion