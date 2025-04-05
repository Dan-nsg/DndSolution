<script setup lang="ts">
import { RouterLink, RouterView, useRoute } from 'vue-router';
import { ref, watch, onMounted } from 'vue';

const route = useRoute();

const isDarkMode = ref(false);

const checkTheme = () => {
  const savedTheme = localStorage.getItem('theme');
  const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

  if (savedTheme) {
    isDarkMode.value = savedTheme === 'dark';
  } else {
    isDarkMode.value = prefersDark;
  }
  applyTheme(isDarkMode.value);
};

const applyTheme = (dark: boolean) => {
  document.documentElement.setAttribute('data-theme', dark ? 'dark' : 'light');
  document.documentElement.classList.add('theme-transition');

  setTimeout(() => {
    document.documentElement.classList.remove('theme-transition');
  }, 500);
};

const toggleTheme = () => {
  localStorage.setItem('theme', isDarkMode.value ? 'dark' : 'light');
};

onMounted(() => {
  checkTheme();

  window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
    if (!localStorage.getItem('theme')) {
      isDarkMode.value = e.matches;
      applyTheme(e.matches);
    }
  });
});

watch(isDarkMode, (newValue) => {
  applyTheme(newValue);
  toggleTheme();
});
</script>

<template>
  <div class="app-container">
    <header>
      <div class="header-content">
        <h1 class="site-title">Grimoire</h1>

        <div class="navigation-tabs">
          <RouterLink to="/" :class="{ active: route.path === '/' }">
            <span class="tab-icon"><i class="fas fa-home"></i></span>
            <span class="tab-text">Home</span>
          </RouterLink>
          <RouterLink to="/spells" :class="{ active: route.path === '/spells' }">
            <span class="tab-icon"><i class="fas fa-magic"></i></span>
            <span class="tab-text">Spells</span>
          </RouterLink>
        </div>

        <div class="theme-toggle">
          <label class="theme-switch" :title="isDarkMode ? 'Switch to Light Mode' : 'Switch to Dark Mode'">
            <input type="checkbox" v-model="isDarkMode" />
            <span class="slider">
              <i class="toggle-icon" :class="isDarkMode ? 'fas fa-moon' : 'fas fa-sun'"></i>
            </span>
          </label>
        </div>
      </div>
    </header>

    <main>
      <RouterView />
    </main>

    <footer>
      <div class="footer-content">
        <p>The Arcane Codex © {{ new Date().getFullYear() }}</p>
      </div>
    </footer>
  </div>
</template>

<style>
:root {
  --color-background: #f5f0e6;
  --color-text: #3a2f28;
  --color-primary: #8b0000;
  --color-secondary: #a87b30;
  --color-border: #d4af37;
  --color-accent: #5e4e35;
  --header-bg: #2c1e16;
  --header-text: #f5f0e6;
  --tab-active-bg: #8b0000;
  --tab-active-text: #f5f0e6;
  --tab-hover-bg: rgba(168, 123, 48, 0.7);
  --shadow-color: rgba(0, 0, 0, 0.25);
  --transition-duration: 0.3s;
  --transition-timing: ease;
}

[data-theme="dark"] {
  --color-background: #1e1c1a;
  --color-text: #e0d7c6;
  --color-primary: #aa3333;
  --color-secondary: #c09844;
  --color-border: #f8d568;
  --color-accent: #7d683e;
  --header-bg: #0f0d0c;
  --header-text: #e0d7c6;
  --tab-active-bg: #aa3333;
  --tab-active-text: #e0d7c6;
  --tab-hover-bg: rgba(192, 152, 68, 0.7);
  --shadow-color: rgba(0, 0, 0, 0.5);
}

.theme-transition *,
.theme-transition *::before,
.theme-transition *::after {
  transition:
    background-color var(--transition-duration) var(--transition-timing),
    color var(--transition-duration) var(--transition-timing),
    border-color var(--transition-duration) var(--transition-timing),
    box-shadow var(--transition-duration) var(--transition-timing);
}

*,
*::before,
*::after {
  box-sizing: border-box;
  margin: 0;
  font-weight: normal;
}

body {
  min-height: 100vh;
  line-height: 1.6;
  font-size: 15px;
  text-rendering: optimizeLegibility;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  background-color: var(--color-background);
  color: var(--color-text);
}

.app-container {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  font-family: 'Cinzel', serif;
  background-image: url("data:image/svg+xml,%3Csvg width='40' height='40' viewBox='0 0 40 40' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M20 20.5V18H0v-2h20v-2H0v-2h20v-2H0V8h20V6H0V4h20V2H0V0h22v20h2V0h2v20h2V0h2v20h2V0h2v20h2v2H20v-1.5zM0 20h2v20H0V20zm4 0h2v20H4V20zm4 0h2v20H8V20zm4 0h2v20h-2V20zm4 0h2v20h-2V20zm4 4h20v2H20v-2zm0 4h20v2H20v-2zm0 4h20v2H20v-2zm0 4h20v2H20v-2z' fill='%23523f2e' fill-opacity='0.05' fill-rule='evenodd'/%3E%3C/svg%3E");
}

header {
  background-color: var(--header-bg);
  color: var(--header-text);
  box-shadow: 0 4px 10px var(--shadow-color);
  border-bottom: 3px solid var(--color-primary);
  padding: 0;
  position: sticky;
  top: 0;
  z-index: 1000;
}

.header-content {
  max-width: 1200px;
  margin: 0 auto;
  padding: 12px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.site-title {
  font-size: 1.8rem;
  margin: 0;
  font-weight: 700;
  letter-spacing: 2px;
  text-transform: uppercase;
  color: var(--color-border);
  text-shadow: 0 0 5px rgba(0, 0, 0, 0.3), 0 0 10px rgba(212, 175, 55, 0.3);
}

.navigation-tabs {
  display: flex;
  gap: 8px;
}

.navigation-tabs a {
  text-decoration: none;
  padding: 8px 16px;
  border-radius: 4px;
  color: var(--header-text);
  font-size: 0.9rem;
  font-weight: 600;
  letter-spacing: 1px;
  text-transform: uppercase;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  border: 1px solid transparent;
}

.navigation-tabs a::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
  transition: all 0.5s ease;
}

.navigation-tabs a:hover::before {
  left: 100%;
}

.navigation-tabs a:hover {
  background-color: var(--tab-hover-bg);
  transform: translateY(-2px);
  border: 1px solid var(--color-border);
}

.navigation-tabs a.active {
  background-color: var(--tab-active-bg);
  color: var(--tab-active-text);
  border: 1px solid var(--color-border);
  box-shadow: 0 0 10px rgba(212, 175, 55, 0.3);
}

.tab-icon {
  font-size: 1.1rem;
}

.theme-toggle {
  display: flex;
  align-items: center;
}

.theme-switch {
  position: relative;
  display: inline-block;
  width: 60px;
  height: 30px;
  margin-left: 20px;
}

.theme-switch input {
  opacity: 0;
  width: 0;
  height: 0;
}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: var(--header-bg);
  transition: 0.4s;
  border-radius: 30px;
  border: 2px solid var(--color-border);
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 10px;
  box-shadow: 0 0 5px rgba(212, 175, 55, 0.5) inset;
}

.toggle-icon {
  font-size: 14px;
  transition: transform 0.4s;
  color: var(--color-border);
}

input:checked+.slider .toggle-icon {
  transform: translateX(30px);
}

.slider:before {
  position: absolute;
  content: "";
  height: 22px;
  width: 22px;
  left: 4px;
  bottom: 2px;
  background: linear-gradient(145deg, var(--color-border), var(--color-secondary));
  transition: 0.4s;
  border-radius: 50%;
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.5);
}

input:checked+.slider:before {
  transform: translateX(30px);
}

.theme-switch input:checked+.slider {
  background-color: var(--header-bg);
}

.theme-switch input:checked+.slider:before {
  transform: translateX(30px);
}

.theme-switch input:checked+.slider .toggle-icon.fa-sun {
  transform: translateX(-30px);
  opacity: 0;
}

.theme-switch input:checked+.slider .toggle-icon.fa-moon {
  transform: translateX(0);
  opacity: 1;
}

.toggle-icon {
  position: absolute;
  transition: all 0.4s ease;
}

.toggle-icon.fa-sun {
  opacity: 1;
  transform: translateX(0);
}

.toggle-icon.fa-moon {
  opacity: 0;
  transform: translateX(30px);
}

main {
  flex: 1;
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
}

footer {
  background-color: var(--header-bg);
  color: var(--header-text);
  border-top: 3px solid var(--color-primary);
  padding: 15px 0;
  text-align: center;
  font-size: 0.8rem;
  margin-top: auto;
}

.footer-content {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

@media (max-width: 768px) {
  .header-content {
    flex-direction: column;
    gap: 12px;
    padding: 10px;
  }

  .site-title {
    font-size: 1.5rem;
  }

  .tab-text {
    display: none;
  }

  .navigation-tabs a {
    padding: 8px 12px;
  }

  .theme-switch {
    margin-left: 0;
    margin-top: 10px;
  }
}
</style>