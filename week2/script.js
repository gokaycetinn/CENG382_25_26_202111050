let userData = [];

function showLogin() {
    let loginForm = document.getElementById('loginForm');
    loginForm.style.display = 'block';
    document.body.style.background = "url('icardi.gif') no-repeat center center/cover";
}

document.addEventListener("DOMContentLoaded", function() {
    document.querySelector('.logo').addEventListener('click', showLogin);
    
    let clock = document.getElementById('clock');
    if (!clock) {
        clock = document.createElement('div');
        clock.id = 'clock';
        document.body.appendChild(clock);
    }
    clock.style.display = 'block';
    updateClock();
    setInterval(updateClock, 1000);
});

function loginUser() {
    let username = document.querySelector("input[placeholder='User']").value;
    let password = document.querySelector("input[placeholder='Password']").value;
    
    if (username && password) {
        userData.push({ username, password });
        console.log("Stored User Data:", userData);
    } 
}
// Updates the clock every second and displays the current time //
function updateClock() {
    let clock = document.getElementById('clock');
    if (!clock) return;
    
    let now = new Date();
    let hours = now.getHours().toString().padStart(2, '0');
    let minutes = now.getMinutes().toString().padStart(2, '0');
    let seconds = now.getSeconds().toString().padStart(2, '0');
    clock.innerText = `${hours}:${minutes}:${seconds}`;
    console.log("Clock updated: ", clock.innerText);
}

// Press 'H' to hide/show all content //
document.addEventListener("keydown", function(event) {
    if (event.key.toLowerCase() === 'h') {
        let allElements = document.body.children;
        for (let i = 0; i < allElements.length; i++) {
            allElements[i].style.display = (allElements[i].style.display === "none" ? "block" : "none");
        }
        console.log("All elements visibility toggled");
    }
});
 