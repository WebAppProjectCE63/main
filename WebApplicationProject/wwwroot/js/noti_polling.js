(function(){
    const POLL_MS = 5000;

    async function fetchUnread(){
        try{
            const r = await fetch('/Notification/UnreadCount?t=' + Date.now(), { cache: 'no-store' });
            if(!r.ok) return;
            const j = await r.json();
            if(!j.success) return;
            const count = j.count || 0;
            const badge = document.getElementById('notiBadge');
            if(!badge) return;
            if(count > 0){
                badge.style.display = 'inline-block';
                badge.innerText = count;
            } else {
                badge.style.display = 'none';
            }
        }catch(e){ console.error('noti poll', e); }
    }

    window.NotiPolling = {
        fetchRecent: async function(limit){
            try{
                const r = await fetch('/Notification/Recent?limit=' + (limit||5) + '&t=' + Date.now(), { cache: 'no-store' });
                if(!r.ok) return [];
                const j = await r.json();
                if(!j.success) return [];
                return j.items || [];
            }catch(e){ console.error('noti recent', e); return []; }
        }
    };

    document.addEventListener('DOMContentLoaded', function(){
        fetchUnread();
        setInterval(fetchUnread, POLL_MS);
    });
})();
