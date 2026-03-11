(function () {
    async function updateStatus() {
        const cards = document.querySelectorAll('.Event');
        if (cards.length === 0) return;

        for (const card of cards) {
            const idInput = card.querySelector('input[name="eventId"]');
            if (!idInput) continue;
            const eventId = idInput.value.trim();

            try {
                const response = await fetch(`/Event/CheckStatus/${eventId}?t=${new Date().getTime()}`, { cache: "no-store" });
                if (!response.ok) continue;
                const data = await response.json();

                const now = new Date();
                const eventStartTime = new Date(data.dateTime);
                const closeTime = new Date(eventStartTime.getTime() - (2 * 60 * 1000));
                const isActuallyLocked = data.isRegistrationClosed || (now >= closeTime);

                // =========================================
                // 1. จัดการป้าย Locked ด้านหน้า
                // =========================================
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

                // =========================================
                // 🚨 2. ท่าไม้ตาย: สแกนปุ่มจาก "ตัวอักษร" โดยตรง
                // =========================================
                const allButtons = card.querySelectorAll('button');

                allButtons.forEach(btn => {
                    const text = btn.textContent.trim();

                    // 🔴 2.1 จัดการปุ่ม "Cancel Join" (หน้า My Event)
                    if (text === 'Cancel Join') {
                        const form = btn.closest('form');
                        if (form) {
                            // ถ้าล็อค ให้ซ่อนฟอร์มทิ้งทันที ถ้าปลดล็อคให้โชว์กลับมา
                            form.style.setProperty('display', isActuallyLocked ? 'none' : 'inline-block', 'important');
                        }
                    }

                    // 🟡 2.2 จัดการปุ่มตัวสำรอง (หน้า My Event)
                    if (text === 'Waiting for Confirmation' || text === 'Event Closed (Not Selected)') {
                        btn.textContent = isActuallyLocked ? 'Event Closed (Not Selected)' : 'Waiting for Confirmation';
                    }

                    // 🟢 2.3 จัดการปุ่ม Join (หน้า Home)
                    if (text === 'Join' || text === 'Closed') {
                        if (isActuallyLocked) {
                            btn.textContent = 'Join';
                            btn.disabled = true;
                            btn.style.backgroundColor = '#6c757d';
                        } else {
                            btn.textContent = 'Join';
                            btn.disabled = false;
                            btn.style.backgroundColor = '';
                        }
                    }
                });

                // =========================================
                // 3. จัดการป้ายมุมขวาบน (Badge) ของตัวสำรอง
                // =========================================
                const badge = card.querySelector('.badge');
                if (badge) {
                    const bText = badge.textContent.trim();
                    if (isActuallyLocked && bText === 'Waiting') {
                        badge.textContent = 'Not Selected';
                        badge.className = 'badge badge-closed';
                    } else if (!isActuallyLocked && bText === 'Not Selected') {
                        badge.textContent = 'Waiting';
                        badge.className = 'badge badge-waiting';
                    }
                }

            } catch (e) {
                console.error("Realtime Error: ", e);
            }
        }
    }

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