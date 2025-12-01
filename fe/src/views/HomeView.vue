<template>
  <div class="content">
    <aside class="sidebar">
      <div class="filters">
        <h3>Book type</h3>
        <label class="filter-pill"><input type="checkbox" v-model="filters.types.book" /><div class="color-box" style="background:#ff6d2f"></div><span>Book</span></label>
        <label class="filter-pill"><input type="checkbox" v-model="filters.types.audio" /><div class="color-box" style="background:#ff6d2f"></div><span>Audio book</span></label>
        <label class="filter-pill"><input type="checkbox" v-model="filters.types.ebook" /><div class="color-box" style="background:#ff6d2f"></div><span>e-book</span></label>

        <h3>Show only</h3>
        <label class="filter-pill"><input type="checkbox" v-model="filters.show.new" /><div class="color-box" style="background:#11b3a8"></div><span>New</span></label>
        <label class="filter-pill"><input type="checkbox" v-model="filters.show.sale" /><div class="color-box" style="background:#11b3a8"></div><span>Sale</span></label>
        <label class="filter-pill"><input type="checkbox" v-model="filters.show.instock" /><div class="color-box" style="background:#11b3a8"></div><span>In stock</span></label>

        <h3>Language options</h3>
        <label class="filter-pill"><input type="checkbox" v-model="filters.lang.en" /><div class="color-box" style="background:#8b0b0b"></div><span>English</span></label>
        <label class="filter-pill"><input type="checkbox" v-model="filters.lang.de" /><div class="color-box" style="background:#8b0b0b"></div><span>German</span></label>
        <label class="filter-pill"><input type="checkbox" v-model="filters.lang.sl" /><div class="color-box" style="background:#8b0b0b"></div><span>Slovene</span></label>
      </div>
    </aside>

    <main class="main">
      <h2 class="section-title">Computer Science books</h2>
      <div class="grid">
        <div class="card" v-for="(book, idx) in books" :key="idx">
          <img :src="book.image" :alt="book.title" />
          <div class="price">{{ book.price }} €</div>
          <button class="buy">BUY</button>
        </div>
      </div>
    </main>
  </div>
</template>

<script>
export default {
  name: 'HomeView',
  data() {
    return {
      books: [
        { title: 'Elements of Computer Science', price: 40, image: require('@/assets/book1.jpg') },
        { title: 'Intro to Computer Science', price: 35, image: require('@/assets/book2.svg') },
        { title: 'Algorithms and Data', price: 57, image: require('@/assets/book3.svg') },
        { title: 'Essential Computer Science', price: 22, image: require('@/assets/book4.svg') },
        { title: 'Programming Essentials', price: 29, image: require('@/assets/book5.svg') },
        { title: 'Foundations of CS', price: 48, image: require('@/assets/book6.svg') }
      ],
      filters: {
        types: { book: true, audio: false, ebook: false },
        show: { new: false, sale: false, instock: true },
        lang: { en: true, de: false, sl: false }
      }
    }
  }
}
</script>

<style scoped>
/* Layout: sidebar + main */
.content{ display: grid; grid-template-columns: 280px 1fr; gap: 28px; align-items: start; }
.sidebar{ background: var(--panel); border-right: 1px solid var(--muted); padding: 18px; }
.filters{ display:flex; flex-direction:column; gap:18px; }
.filters h3{ margin:0 0 8px 0; font-size:1rem; }

.filter-pill{ display:flex; align-items:center; gap:10px; padding:6px 6px; cursor:pointer; }
.filter-pill input{ width:16px; height:16px; margin-right:6px; }
.filter-pill span{ font-size:0.95rem; }
.color-box{ width:14px; height:14px; border-radius:3px; }

/* Main area */
.main{ background: transparent; }
.grid{ display:flex; flex-wrap:wrap; gap:48px; align-items: flex-start; }
.card{ width: 240px; background: white; box-shadow: 0 1px 0 rgba(0,0,0,0.02); padding: 14px; text-align:center; border-radius:12px; overflow:hidden; }
.card img{ width: 100%; height: 300px; object-fit: cover; margin-bottom: 12px; border-radius:6px; }
.price{ display:inline-block; background: rgba(255,109,47,0.08); color: var(--accent); font-weight:700; margin-bottom:8px; font-size:1.1rem; padding:6px 14px; border-radius:999px; }
.buy{ border-radius:18px; border:2px solid #cfcfcf; background: transparent; padding:6px 14px; color:#777; cursor:pointer; }

/* Sections separation inside sidebar */
.filters > div + h3{ margin-top: 12px; }

@media (max-width: 900px){
  .content{ grid-template-columns: 1fr; }
  .sidebar{ order:2 }
  .main{ order:1 }
  .card img{ height:160px }
}
</style>
