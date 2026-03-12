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
                const closeTime = new Date(data.registrationDeadline);
                const isActuallyLocked = data.isRegistrationClosed || (now >= closeTime);

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

                const allButtons = card.querySelectorAll('button');

                allButtons.forEach(btn => {
                    const text = btn.textContent.trim();

                    if (text === 'Cancel Join') {
                        const form = btn.closest('form');
                        if (form) {
                            form.style.setProperty('display', isActuallyLocked ? 'none' : 'inline-block', 'important');
                        }
                    }

                    if (text === 'Waiting for Confirmation' || text === 'Event Closed (Not Selected)') {
                        btn.textContent = isActuallyLocked ? 'Event Closed (Not Selected)' : 'Waiting for Confirmation';
                    }

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