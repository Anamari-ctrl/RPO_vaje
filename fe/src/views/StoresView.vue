<template>
    <nav class="nav">
        <div class="nav-1">
            <div class="tip-wrap">
                <router-link class="item" to="/profile" aria-describedby="tt-profile">👤 Profile</router-link>
                <span id="tt-profile" role="tooltip" class="tooltip">View and edit your profile</span>
            </div>

            <div class="tip-wrap">
                <router-link class="item" to="/" aria-describedby="tt-wishlist">❤︎ Wishlist</router-link>
                <span id="tt-wishlist" role="tooltip" class="tooltip">See your saved books</span>
            </div>
        </div>
    </nav>


    <div class="stores-view">
        <h1 class="page-title">Our Stores</h1>
        <p class="subtitle">Visit us at any of our locations across Slovenia</p>

        <div v-if="loading" class="loading">Loading stores...</div>

        <div v-else class="stores-grid">
            <div v-for="store in stores" :key="store.id" class="store-card">
                <div class="store-header">
                    <h2>{{ store.title }}</h2>
                    <p class="address">📍 {{ store.location.address }}</p>
                </div>

                <div class="store-content">
                <img 
                  :src="`/storeImages/bookstore${store.id}.jpg`" 
                  :alt="`Store Image ${store.id}`"
                >
                    <!-- Working Hours -->
                    <div class="section">
                        <h3>🕒 Working Hours</h3>
                        <div class="hours-list">
                            <div v-for="(hours, day) in store.workingHours" :key="day" class="hours-row">
                                <span class="day">{{ capitalize(day) }}:</span>
                                <span class="hours">{{ hours }}</span>
                            </div>
                        </div>
                    </div>

                    <!-- Contact -->
                    <div class="section">
                        <h3>📞 Contact</h3>
                        <div class="contact-info">
                            <p><strong>Phone:</strong> <a :href="`tel:${store.contact.phone}`">{{ store.contact.phone }}</a></p>
                            <p><strong>Email:</strong> <a :href="`mailto:${store.contact.email}`">{{ store.contact.email }}</a></p>
                        </div>
                    </div>

                    <!-- Manager -->
                    <div class="section">
                        <h3>👤 Store Manager</h3>
                        <div class="manager-info">
                            <p><strong>{{ store.manager.name }}</strong></p>
                            <p><a :href="`mailto:${store.manager.email}`">{{ store.manager.email }}</a></p>
                        </div>
                    </div>

                    <!-- Notices -->
                    <div class="section" v-if="store.notices && store.notices.length > 0">
                        <h3>📢 Notices</h3>
                        <ul class="notices-list">
                            <li v-for="(notice, index) in store.notices" :key="index">{{ notice }}</li>
                        </ul>
                    </div>

                    <!-- Map -->
                    <div class="section map-section">
                        <h3>🗺️ Location</h3>
                        <div class="map-container">
                            <iframe :src="`https://www.openstreetmap.org/export/embed.html?bbox=${store.location.lng - 0.01},${store.location.lat - 0.01},${store.location.lng + 0.01},${store.location.lat + 0.01}&layer=mapnik&marker=${store.location.lat},${store.location.lng}`"
                                    width="100%"
                                    height="250"
                                    frameborder="0"
                                    style="border: 1px solid #ddd; border-radius: 8px;"></iframe>
                            <a :href="`https://www.openstreetmap.org/?mlat=${store.location.lat}&mlon=${store.location.lng}#map=16/${store.location.lat}/${store.location.lng}`"
                               target="_blank"
                               class="map-link">
                                Open in OpenStreetMap →
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import storesService from '../services/stores-service';

export default {
  name: 'StoresView',
  setup() {
    const stores = ref([]);
    const loading = ref(true);

    const fetchStores = async () => {
      try {
        loading.value = true;
        const response = await storesService.getStores();
        stores.value = response.stores;
      } catch (error) {
        console.error('Error fetching stores:', error);
        alert('Failed to load stores. Please try again later.');
      } finally {
        loading.value = false;
      }
    };

    const capitalize = (str) => {
      return str.charAt(0).toUpperCase() + str.slice(1);
    };

    onMounted(() => {
      fetchStores();
    });

    return {
      stores,
      loading,
      capitalize
    };
  }
};
</script>

<style scoped>
.stores-view {
  max-width: 1400px;
  margin: 0 auto;
  padding: 40px 20px;
}

.page-title {
  font-size: 2.5rem;
  color: #2c3e50;
  text-align: center;
  margin-bottom: 10px;
}

.subtitle {
  text-align: center;
  color: #7f8c8d;
  font-size: 1.1rem;
  margin-bottom: 40px;
}

.loading {
  text-align: center;
  font-size: 1.2rem;
  color: #7f8c8d;
  padding: 60px 0;
}

.stores-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(500px, 1fr));
  gap: 30px;
}

.store-card {
  background: white;
  border: 1px solid #e0e0e0;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s, box-shadow 0.2s;
}

.store-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
}

.store-header {
  background: linear-gradient(135deg, #42b983 0%, #35a372 100%);
  color: white;
  padding: 25px;
}

.store-header h2 {
  margin: 0 0 10px 0;
  font-size: 1.6rem;
}

.address {
  margin: 0;
  font-size: 1rem;
  opacity: 0.95;
}

.store-content {
  padding: 25px;
}

.section {
  margin-bottom: 25px;
}

.section:last-child {
  margin-bottom: 0;
}

.section h3 {
  color: #2c3e50;
  font-size: 1.2rem;
  margin: 0 0 15px 0;
  padding-bottom: 8px;
  border-bottom: 2px solid #42b983;
}

.hours-list {
  display: grid;
  gap: 8px;
}

.hours-row {
  display: flex;
  justify-content: space-between;
  padding: 8px 12px;
  background: #f8f9fa;
  border-radius: 6px;
}

.day {
  font-weight: 600;
  color: #2c3e50;
  min-width: 100px;
}

.hours {
  color: #555;
}

.contact-info p,
.manager-info p {
  margin: 8px 0;
  color: #555;
}

.contact-info a,
.manager-info a {
  color: #42b983;
  text-decoration: none;
}

.contact-info a:hover,
.manager-info a:hover {
  text-decoration: underline;
}

.notices-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.notices-list li {
  padding: 10px 15px;
  margin-bottom: 8px;
  background: #fff3cd;
  border-left: 4px solid #ffc107;
  border-radius: 4px;
  color: #856404;
}

.notices-list li:last-child {
  margin-bottom: 0;
}

.map-section {
  margin-top: 20px;
}

.map-container {
  position: relative;
}

.map-link {
  display: inline-block;
  margin-top: 10px;
  color: #42b983;
  text-decoration: none;
  font-weight: 500;
}

.map-link:hover {
  text-decoration: underline;
}

@media (max-width: 768px) {
  .stores-grid {
    grid-template-columns: 1fr;
  }

  .page-title {
    font-size: 2rem;
  }

  .store-header h2 {
    font-size: 1.4rem;
  }
}

    .item {
        text-decoration: none;
        color: var(--text);
        font-weight: 500;
        padding: 6px 10px;
        border-radius: 6px;
        transition: background 0.2s;
    }

        .item:hover {
            background: rgba(0,0,0,0.05);
        }

    .nav {
        display: flex;
        align-items: center;
        padding: 8px 16px;
        background: var(--panel);
        border-bottom: 1px solid var(--muted);
        width: 100%;
    }

    /* push this container to the RIGHT */
    .nav-1 {
        margin-left: auto;
        display: flex;
        gap: 20px;
        align-items: center;
    }
    .tip-wrap {
        position: relative;
        display: inline-flex;
        align-items: center;
    }

    .tooltip {
        position: absolute;
        top: calc(100% + 8px);
        left: 50%;
        transform: translateX(-50%) translateY(-4px);
        padding: 6px 10px;
        border-radius: 10px;
        background: #111;
        color: #fff;
        font-size: 12px;
        line-height: 1.2;
        white-space: nowrap;
        opacity: 0;
        visibility: hidden;
        pointer-events: none;
        transition: opacity 120ms ease, transform 120ms ease, visibility 120ms ease;
        z-index: 1000;
    }

        .tooltip::after {
            content: "";
            position: absolute;
            top: -6px;
            left: 50%;
            transform: translateX(-50%);
            border-width: 6px;
            border-style: solid;
            border-color: transparent transparent #111 transparent;
        }

    .tip-wrap:hover > .tooltip,
    .tip-wrap:focus-within > .tooltip {
        opacity: 1;
        visibility: visible;
        transform: translateX(-50%) translateY(0);
    }

    /* Reduced motion */
    @media (prefers-reduced-motion: reduce) {
        .tooltip {
            transition: none;
        }
    }

</style>
