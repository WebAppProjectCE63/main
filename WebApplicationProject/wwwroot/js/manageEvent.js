function removeUser(eventId, userId) {
    if (!confirm("ยืนยันการลบผู้ใช้นี้ออกจากตัวจริง?")) {
        return;
    }
    fetch(`/Event/RemoveParti?eventId=${eventId}&userId=${userId}`, { method: 'POST' })
        .then(async response => {
            if (response.ok) {
                const countSpan = document.getElementById("currentParti");
                let currentNumber = parseInt(countSpan.innerText);
                const rowToRemove = document.getElementById(`row_${userId}`);
                countSpan.innerText = currentNumber - 1;
                rowToRemove.remove();
                const confirmedSeqNumbers = document.querySelectorAll('#confirmedTable .seqNumber');
                confirmedSeqNumbers.forEach((td, index) => {
                    td.innerText = index + 1;
                })
            } else {
                const errorMsg = await response.text();
                alert(errorMsg);
            }
        })
        .catch(error => {
            console.error("Fetch Error:", error);
            alert("ไม่สามารถติดต่อระบบหลังบ้านได้ กรุณาลองใหม่อีกครั้ง");
        });
}
function promoteUser(eventId, userId) {
    if (!confirm("ยืนยันการเพิ่มผู้ใช้นี้เข้าสู่ตัวจริง?")) {
        return;
    }
    fetch(`/Event/PromoteParti?eventId=${eventId}&userId=${userId}`, { method: 'POST' })
        .then(async response => {
            if (response.ok) {
                const countSpan = document.getElementById("currentParti");
                let currentNumber = parseInt(countSpan.innerText);
                const confirmedTable = document.getElementById("confirmedTable");
                const rowToPromote = document.getElementById(`row_${userId}`);
                confirmedTable.appendChild(rowToPromote);
                const actionBtn = rowToPromote.querySelector("button");
                actionBtn.setAttribute("onclick", `removeUser(${eventId}, ${userId})`);
                actionBtn.innerText = "ลบ";
                countSpan.innerText = currentNumber + 1;
                const confirmedSeqNumbers = confirmedTable.querySelectorAll(".seqNumber");
                confirmedSeqNumbers.forEach((td, index) => {
                    td.innerText = index + 1;
                })
                const waitingSeqNumbers = document.querySelectorAll("#waitingTable .seqNumber")
                waitingSeqNumbers.forEach((td, index) => {
                    td.innerText = index + 1;
                })
            } else {
                const errorMsg = await response.text();
                alert(errorMsg);
            }
        })
        .catch(error => {
            console.error("Fetch Error:", error);
            alert("ไม่สามารถติดต่อระบบหลังบ้านได้ กรุณาลองใหม่อีกครั้ง");
        });
}