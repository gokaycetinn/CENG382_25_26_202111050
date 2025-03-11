document.getElementById("classForm").addEventListener("submit", function(event) {
    event.preventDefault(); // Sayfa yenilenmesini önle

    let className = document.getElementById("className").value;
    let studentCount = document.getElementById("studentCount").value;
    let description = document.getElementById("description").value;

    let tableBody = document.getElementById("classTableBody");
    let newRow = document.createElement("tr");
    newRow.innerHTML = `<td>${className}</td><td>${studentCount}</td><td>${description}</td>`;

    // 🔹 1. Olay: Satıra tıklanınca arka plan rengini değiştir
    newRow.addEventListener("click", function() {
        this.style.backgroundColor = this.style.backgroundColor === "rgba(241, 11, 11, 0.48)" 
            ? "transparent" 
            : "rgba(241, 11, 11, 0.48)";
    });
    
    // 🔹 2. Olay: Çift tıklanınca satırı sil
    newRow.addEventListener("dblclick", function() {
        this.remove();
    });
    newRow.addEventListener("mouseenter", function() {
        this.style.backgroundColor = "rgba(13, 13, 14, 0.3)"; 
    });
    newRow.addEventListener("mouseleave", function() {
        this.style.backgroundColor = "transparent";
    });

    tableBody.appendChild(newRow);
    document.getElementById("classForm").reset(); 
});

// 🔹 3. Olay: Tüm sınıfları temizleme butonu ekle
document.addEventListener("DOMContentLoaded", function() {
    let clearButton = document.createElement("button");
    clearButton.textContent = "Clear All Classes";
    clearButton.style.marginTop = "15px";
    clearButton.style.padding = "10px";
    clearButton.style.backgroundColor = "red";
    clearButton.style.color = "white";
    clearButton.style.border = "none";
    clearButton.style.cursor = "pointer";
    clearButton.style.borderRadius = "5px";

    clearButton.addEventListener("click", function() {
        document.getElementById("classTableBody").innerHTML = "";
    });

    document.querySelector(".container").appendChild(clearButton);
});
