// บันทึกลงใน wwwroot/js/realtime_update.js
(function () {
    async function updateStatus() {
        const cards = document.querySelectorAll('.Event');
        if (cards.length === 0) return;

        for (const card of cards) {
            const idInput = card.querySelector('input[name="eventId"]');
            if (!idInput) continue;
            const eventId = idInput.value;

            try {
                // บังคับทำลาย Cache ด้วย Timestamp
                const response = await fetch(`/Event/CheckStatus/${eventId}?t=${new Date().getTime()}`, {
                    cache: "no-store"
                });

                if (!response.ok) continue;
                const data = await response.json();

                const now = new Date();
                const eventStartTime = new Date(data.dateTime);
                const closeTime = new Date(eventStartTime.getTime() - (2 * 60 * 1000));
                const isActuallyLocked = data.isRegistrationClosed || (now >= closeTime);
                const isOngoing = now >= eventStartTime;

                // 1. จัดการป้าย Locked
                const imgCol = card.querySelector('.image-column');
                let lockedBadge = card.querySelector('.statis-close');
                if (isActuallyLocked) {
                    if (imgCol && !lockedBadge) {
                        const span = document.createElement('span');
                        span.className = 'status-badge statis-close';
                        span.style.cssText = 'width: 100%; background-color: #dc3545; color: white; text-align: center; display: block; padding: 2px; margin-top: -4px; font-size: 0.85em;';
                        span.innerHTML = '🔒 Locked';
                        imgCol.appendChild(span);
                    }
                } else if (lockedBadge) {
                    lockedBadge.remove();
                }

                // 2. จัดการปุ่ม Join (หน้า Home)
                const joinBtn = card.querySelector('form[action*="Join"] button[type="submit"]');
                if (joinBtn) {
                    if (isActuallyLocked) {
                        joinBtn.textContent = "Closed";
                        joinBtn.disabled = true;
                        joinBtn.style.backgroundColor = '#6c757d';
                    } else {
                        joinBtn.textContent = "Join";
                        joinBtn.disabled = false;
                        joinBtn.style.backgroundColor = '';
                    }
                }

                // 3. ซ่อนปุ่ม Host เมื่อ Ongoing
                if (isOngoing) {
                    card.querySelectorAll('a[href*="Manage"], a[href*="Edit"], form[action*="Delete"]').forEach(el => el.style.display = 'none');
                }
            } catch (e) { }
        }
    }

    // รันทันทีที่โหลดเสร็จและวนลูป
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            updateStatus();
            setInterval(updateStatus, 5000);
        });
    } else {
        updateStatus();
        setInterval(updateStatus, 5000);
    }
})();