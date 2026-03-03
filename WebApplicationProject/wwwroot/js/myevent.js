
document.addEventListener('DOMContentLoaded', function () {
    const monthSelect = document.getElementById('monthSelect');
    const yearSelect = document.getElementById('yearSelect');
    const calendarGrid = document.getElementById('calendarGrid');
    const eventCards = document.querySelectorAll('.event-card');
    const selectedDateText = document.getElementById('selectedDateText');
    const sectionHeader = document.getElementById('sectionHeader');
    const searchInput = document.getElementById('searchInput');
    let currentSelectedDate = null;

    function renderCalendar() {
        const year = parseInt(yearSelect.value);
        const month = parseInt(monthSelect.value);
        const firstDay = new Date(year, month, 1).getDay();
        const daysInMonth = new Date(year, month + 1, 0).getDate();
        const today = new Date();
        const todayDateStr = `${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}-${String(today.getDate()).padStart(2, '0')}`;
        calendarGrid.innerHTML = '';

        for (let i = 0; i < firstDay; i++) {
            const span = document.createElement('span');
            span.className = 'text-muted';
            calendarGrid.appendChild(span);
        }

        for (let day = 1; day <= daysInMonth; day++) {
            const span = document.createElement('span');
            span.textContent = day;
            span.style.cursor = 'pointer';

            const dateStr = `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
            if (dateStr === todayDateStr) {
                span.classList.add('highlight-day');
                span.title = "Today";
            }
            if (currentSelectedDate === dateStr) {
                span.classList.add('active-day');
            }

            span.addEventListener('click', () => {
                if (currentSelectedDate === dateStr) {
                    currentSelectedDate = null;
                    span.classList.remove('active-day');
                } else {
                    document.querySelectorAll('#calendarGrid span').forEach(s => s.classList.remove('active-day'));
                    span.classList.add('active-day');
                    currentSelectedDate = dateStr;
                }
                applyFilters(); 
            });

            calendarGrid.appendChild(span);
        }
    }

    function applyFilters() {
        const searchText = searchInput ? searchInput.value.toLowerCase().trim() : '';
        let found = 0;

        eventCards.forEach(card => {
            const dateStr = card.getAttribute('data-date');
            const title = card.querySelector('h3') ? card.querySelector('h3').textContent.toLowerCase() : '';

            const matchDate = !currentSelectedDate || dateStr === currentSelectedDate;
            const matchSearch = !searchText || title.includes(searchText);

            if (matchDate && matchSearch) {
                card.style.display = 'flex';
                found++;
            } else {
                card.style.display = 'none';
            }
        });

        if (sectionHeader) sectionHeader.textContent = found > 0 ? "Events" : "No events found";
        if (selectedDateText) {
            selectedDateText.textContent = currentSelectedDate ? `Events for: ${new Date(currentSelectedDate).toLocaleDateString('en-GB')}` : "All Events";
        }
    }

    const sortSelect = document.getElementById('sortSelect');
    const eventContainer = document.querySelector('.recommend-section');

    if (sortSelect && eventContainer) {
        sortSelect.addEventListener('change', function () {
            const cards = Array.from(document.querySelectorAll('.event-card'));
            const sortBy = this.value;

            cards.sort((a, b) => {
                const dateA = new Date(a.getAttribute('data-date') || 0);
                const dateB = new Date(b.getAttribute('data-date') || 0);

                const memA = a.querySelector('.members') ? parseInt(a.querySelector('.members').textContent.split('/')[0]) || 0 : 0;
                const memB = b.querySelector('.members') ? parseInt(b.querySelector('.members').textContent.split('/')[0]) || 0 : 0;

                const nameA = a.querySelector('h3') ? a.querySelector('h3').textContent.trim() : '';
                const nameB = b.querySelector('h3') ? b.querySelector('h3').textContent.trim() : '';

                switch (sortBy) {
                    case 'date-asc':
                        return dateA - dateB; 
                    case 'date-desc':
                        return dateB - dateA; 
                    case 'members-asc':
                        return memA - memB; 
                    case 'members-desc':
                        return memB - memA; 
                    case 'name-asc':
                        return nameA.localeCompare(nameB); 
                    case 'name-desc':
                        return nameB.localeCompare(nameA); 
                    default:
                        return 0;
                }
            });

            cards.forEach(card => eventContainer.appendChild(card));
        });

        sortSelect.dispatchEvent(new Event('change'));
    }
    if (searchInput) {
        searchInput.addEventListener('input', applyFilters);
    }
 
    const today = new Date();
    monthSelect.value = today.getMonth();
    yearSelect.value = today.getFullYear();
    renderCalendar();

    monthSelect.addEventListener('change', renderCalendar);
    yearSelect.addEventListener('change', renderCalendar);
});