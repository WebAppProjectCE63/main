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
                countSpan.innerText = currentNumber + 1;

                const confirmedTable = document.getElementById("confirmedTable");
                const rowToPromote = document.getElementById(`row_${userId}`);
                confirmedTable.appendChild(rowToPromote);

                const actionBtn = rowToPromote.querySelector("button");
                actionBtn.setAttribute("onclick", `removeUser(${eventId}, ${userId})`);
                actionBtn.innerText = "ลบ";

                const waitSpan = document.getElementById("currentWait");
                if (waitSpan) {
                    let currentWaitNumber = parseInt(waitSpan.innerText);
                    waitSpan.innerText = currentWaitNumber - 1;
                }

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
function removeWaitingUser(eventId, userId) {
    if (!confirm("ยืนยันการปฏิเสธผู้ใช้นี้ออกจากรายชื่อสำรอง?")) {
        return;
    }
    fetch(`/Event/RemoveWaiting?eventId=${eventId}&userId=${userId}`, { method: 'POST' })
        .then(async response => {
            if (response.ok) {
                const waitSpan = document.getElementById("currentWait");
                let currentWaitNumber = parseInt(waitSpan.innerText);
                waitSpan.innerText = currentWaitNumber - 1;

                const rowToRemove = document.getElementById(`row_${userId}`);
                rowToRemove.remove();

                const waitingSeqNumbers = document.querySelectorAll('#waitingTable .seqNumber');
                waitingSeqNumbers.forEach((td, index) => {
                    td.innerText = index + 1;
                });
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
function toggleRegistrationStatus(eventId) {
    const btn = document.getElementById('toggleRegBtn');

    const isCurrentlyClosed = btn.innerText.includes('เปิดรับสมัครอีกครั้ง');

    if (!isCurrentlyClosed) {
        if (!confirm('แน่ใจหรือไม่ว่าต้องการปิดรับสมัคร? กิจกรรมจะหายไปจากหน้าแรก')) {
            return; 
        }
    }

    fetch(`/Event/ToggleRegistration?id=${eventId}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                if (data.isClosed) {
                    btn.innerHTML = '✅ เปิดรับสมัครอีกครั้ง';
                    btn.classList.remove('btn-toggle-close');
                    btn.classList.add('btn-toggle-open');
                } else {
                    btn.innerHTML = '❌ ปิดรับสมัคร';
                    btn.classList.remove('btn-toggle-open');
                    btn.classList.add('btn-toggle-close');
                }
            } else {
                alert(data.message || 'เกิดข้อผิดพลาด');
            }
        })
        .catch(error => {
            console.error('Fetch Error:', error);
            alert("ไม่สามารถติดต่อระบบหลังบ้านได้ กรุณาลองใหม่อีกครั้ง");
        });
}