//dlieu menu
const MENU = {
    burger: [
        { name: "Smash Burger Bò",      price: "149.000đ" },
        { name: "Burger Gà Giòn",       price: "129.000đ" },
        { name: "Double Cheese Burger", price: "169.000đ" },
        { name: "Burger Tôm Tempura",   price: "139.000đ" },
        { name: "Mushroom Swiss Burger",price: "159.000đ" },
        { name: "Wrap Gà Teriyaki",     price: "109.000đ" },
    ],
    ga: [
        { name: "Gà Rán 2 Miếng",    price: "99.000đ"  },
        { name: "Cánh Gà Chiên Mắm", price: "79.000đ"  },
        { name: "Gà Rán Phô Mai",    price: "115.000đ" },
        { name: "Cánh Gà Sốt Cam",   price: "89.000đ"  },
        { name: "Đùi Gà Nướng BBQ",  price: "95.000đ"  },
    ],
    pizza: [
        { name: "Pizza Hải Sản",    price: "199.000đ" },
        { name: "Pizza Bò BBQ",     price: "189.000đ" },
        { name: "Pizza 4 Phô Mai",  price: "219.000đ" },
        { name: "Pizza Gà Nấm",     price: "179.000đ" },
        { name: "Pizza Xúc Xích Ý", price: "185.000đ" },
    ],
    uong: [
        { name: "Coca Cola",          price: "25.000đ" },
        { name: "Trà Sữa Trân Châu", price: "45.000đ" },
        { name: "Sinh Tố Dâu",       price: "49.000đ" },
        { name: "Nước Cam Ép",       price: "39.000đ" },
    ],
    trangmieng: [
        { name: "Bánh Lava Nutella",     price: "89.000đ" },
        { name: "Kem Gelato Dâu",        price: "55.000đ" },
        { name: "Cheesecake Việt Quất",  price: "75.000đ" },
        { name: "Bánh Donut Sô Cô La",  price: "35.000đ" },
    ],
};

//quy tắc trl
const RULES = [
    //chào
    {
        keys: ["xin chào", "chào", "hello", "hi", "hey"],
        reply: () => `Xin chào!Mình là <b>FastBot</b>, trợ lý gợi ý món của FastFood.<br>
            Bạn muốn ăn gì hôm nay? Gõ tên món hoặc hỏi mình nha!<br><br>
            <b>Gợi ý nhanh:</b> burger · gà rán · pizza · đồ uống · tráng miệng`
    },

    //buger
    {
        keys: ["burger", "bò", "smash", "double cheese", "tôm", "mushroom", "wrap"],
        reply: () => {
            const list = MENU.burger.map(m =>
                `• <b>${m.name}</b> — ${m.price}`
            ).join("<br>");
            return `<b>Các món Burger của chúng mình:</b><br>${list}<br><br>Bạn muốn xem chi tiết món nào không?`;
        }
    },

    //gà
    {
        keys: ["gà", "ga", "cánh gà", "đùi gà", "chiên mắm", "bbq"],
        reply: () => {
            const list = MENU.ga.map(m =>
                `• <b>${m.name}</b> — ${m.price}`
            ).join("<br>");
            return `<b>Các món Gà Rán:</b><br>${list}<br><br>Giòn rụm lắm đó, thử đi!`;
        }
    },

    //pizza
    {
        keys: ["pizza", "phô mai", "hải sản", "xúc xích"],
        reply: () => {
            const list = MENU.pizza.map(m =>
                `• <b>${m.name}</b> — ${m.price}`
            ).join("<br>");
            return `<b>Các món Pizza:</b><br>${list}<br><br>Pizza đế mỏng giòn, phô mai kéo sợi cực đỉnh!`;
        }
    },

    //drink
    {
        keys: ["uống", "nước", "coca", "trà sữa", "sinh tố", "cam", "đồ uống"],
        reply: () => {
            const list = MENU.uong.map(m =>
                `• <b>${m.name}</b> — ${m.price}`
            ).join("<br>");
            return `<b>Đồ uống:</b><br>${list}<br><br>Kết hợp với burger thì ngon hết sảy!`;
        }
    },

    //trasg miệng
    {
        keys: ["tráng miệng", "bánh", "kem", "dessert", "ngọt", "lava", "donut", "cheesecake"],
        reply: () => {
            const list = MENU.trangmieng.map(m =>
                `• <b>${m.name}</b> — ${m.price}`
            ).join("<br>");
            return `<b>Tráng miệng:</b><br>${list}<br><br>Kết thúc bữa ăn ngọt ngào nhé!`;
        }
    },

    //combo tiết kiệm
    {
        keys: ["rẻ", "tiết kiệm", "ít tiền", "rẻ nhất", "giá thấp", "sinh viên"],
        reply: () => `<b>Gợi ý tiết kiệm nhất:</b><br>
                    • <b>Coca Cola</b> — 25.000đ<br>
                    • <b>Bánh Donut Sô Cô La</b> — 35.000đ<br>
                    • <b>Khoai Tây Chiên</b> — 39.000đ<br>
                    • <b>Cánh Gà Chiên Mắm</b> — 79.000đ<br><br>
                    Combo gà + nước chỉ ~100k thôi! 🎉`
    },

    //bán đắt nhất
    {
        keys: ["ngon nhất", "best", "nổi bật", "bán chạy", "recommend", "gợi ý", "nên ăn gì"],
        reply: () => `⭐ <b>Món được yêu thích nhất:</b><br>
                    • <b>Smash Burger Bò</b> — 149.000đ<br>
                    • <b>Gà Rán Phô Mai</b> — 115.000đ<br>
                    • <b>Pizza 4 Phô Mai</b> — 219.000đ<br>
                    • <b>Bánh Lava Nutella</b> — 89.000đ<br><br>
                    Đây là top bán chạy nhất tháng này!`
    },

    //giờ mở cửa
    {
        keys: ["giờ", "mở cửa", "đóng cửa", "open", "close", "mấy giờ"],
        reply: () => `FastFood mở cửa <b>07:00 – 22:00</b> mỗi ngày (Thứ 2 – Chủ nhật).<br>
        Giao hàng trong vòng <b>25 phút</b> tại Đà Lạt!`
    },

    //dchi
    {
        keys: ["địa chỉ", "ở đâu", "địa điểm", "location", "đường", "quán"],
        reply: () => `Chúng mình ở:<br>
        <b>281 Hai Bà Trưng, Phường 6, Đà Lạt</b><br>
        Hotline: <b>0931 323 316</b><br>
        Email: hello@fastfood.vn`
    },

    //giao hàng 
    {
        keys: ["giao hàng", "ship", "delivery", "giao", "bao lâu"],
        reply: () => `FastFood giao hàng <b>siêu tốc trong 25 phút</b> tại Đà Lạt.<br>
        Đặt hàng ngay trên web là xong, không cần ra ngoài!`
    },

    //tks
    {
        keys: ["cảm ơn", "thanks", "thank you", "ok", "được rồi"],
        reply: () => `Không có gì! Chúc bạn ngon miệng nhé!<br>
        Nếu cần gì thêm cứ hỏi mình nha`
    },

    //combo
    {
        keys: ["combo", "set", "bộ", "phần ăn"],
        reply: () => `<b>Gợi ý combo ngon:</b><br>
        • Burger Bò + Coca Cola = <b>~174.000đ</b><br>
        • Gà Rán 2 Miếng + Nước Cam = <b>~138.000đ</b><br>
        • Pizza Hải Sản + Trà Sữa = <b>~244.000đ</b><br><br>
        Bạn muốn đặt combo nào?`
    },
];

//trl mdinh
const DEFAULT_REPLIES = [
    "Mình chưa hiểu bạn hỏi gì Thử gõ: <b>burger</b>, <b>gà rán</b>, <b>pizza</b>, <b>đồ uống</b>, hoặc <b>gợi ý</b> nhé!",
    "Bạn có thể hỏi mình về món ăn, giá cả, giờ mở cửa hoặc giao hàng nha!",
    "Mình không hiểu lắm Gõ <b>menu</b> để xem danh sách món nhé!",
];

//hàm xly
function getBotReply(userText) {
    const text = userText.toLowerCase().trim();

    //find rule phù hợp
    for (const rule of RULES) {
        if (rule.keys.some(k => text.includes(k))) {
            return rule.reply();
        }
    }

    //nếu gõ menu thì hiện ra all thực đơn
    if (text === "menu" || text === "thực đơn" || text === "all") {
        return `<b>Thực đơn FastFood:</b><br>
            <b>Burger</b> (6 món) — từ 109.000đ<br>
            <b>Gà rán</b> (5 món) — từ 79.000đ<br>
            <b>Pizza</b> (5 món) — từ 179.000đ<br>
            <b>Đồ uống</b> (4 món) — từ 25.000đ<br>
            <b>Tráng miệng</b> (4 món) — từ 35.000đ<br><br>
            Gõ tên danh mục để xem chi tiết!`;
    }

    //nếu kh khớp thì trl ngẫu nhiên
    return DEFAULT_REPLIES[Math.floor(Math.random() * DEFAULT_REPLIES.length)];
}

//khởi tạo bot chat
document.addEventListener("DOMContentLoaded", () => {
    const toggle  = document.getElementById("chatToggle");
    const box     = document.getElementById("chatBox");
    const closeBtn= document.getElementById("chatClose");
    const input   = document.getElementById("chatInput");
    const sendBtn = document.getElementById("chatSend");
    const messages= document.getElementById("chatMessages");

    if (!toggle) return; //k có bot ở đây

    //mở/đóng bot
    toggle.addEventListener("click", () => {
        const isOpen = box.classList.toggle("open");
        toggle.querySelector("i").className =
            isOpen ? "ti ti-x" : "ti ti-message-chatbot";

        //tin nhắn chào lần đầu
        if (isOpen && messages.children.length === 0) {
            addMessage("bot",
                `Xin chào! Mình là <b>FastBot</b>!<br>
                Gõ tên món hoặc hỏi <b>gợi ý</b> để mình giúp bạn chọn bữa ăn nhé 🍔`
            );
        }
    });

    closeBtn.addEventListener("click", () => {
        box.classList.remove("open");
        toggle.querySelector("i").className = "ti ti-message-chatbot";
    });

    //gửi tn
    function sendMessage() {
        const text = input.value.trim();
        if (!text) return;

        addMessage("user", text);
        input.value = "";

        //delay khi ddag gõ
        addTyping();
        setTimeout(() => {
            removeTyping();
            addMessage("bot", getBotReply(text));
        }, 600);
    }

    sendBtn.addEventListener("click", sendMessage);
    input.addEventListener("keydown", e => {
        if (e.key === "Enter") sendMessage();
    });

    //qick btn
    document.querySelectorAll(".chat-quick-btn").forEach(btn => {
        btn.addEventListener("click", () => {
            input.value = btn.dataset.msg;
            sendMessage();
        });
    });

    //thêm tn
    function addMessage(role, html) {
        const div = document.createElement("div");
        div.className = `chat-msg chat-msg--${role}`;
        div.innerHTML = `<div class="chat-bubble">${html}</div>`;
        messages.appendChild(div);
        messages.scrollTop = messages.scrollHeight;
    }

    //cái khi đang gõ 
    function addTyping() {
        const div = document.createElement("div");
        div.className = "chat-msg chat-msg--bot chat-typing";
        div.innerHTML = `<div class="chat-bubble">
            <span class="dot"></span><span class="dot"></span><span class="dot"></span>
        </div>`;
        messages.appendChild(div);
        messages.scrollTop = messages.scrollHeight;
    }

    function removeTyping() {
        const t = messages.querySelector(".chat-typing");
        if (t) t.remove();
    }
});
