<template>
  <div class="spells-container">
    <div class="spells-header">
      <h1 class="spells-title">Collection of Arcane Knowledge</h1>
      <p class="spells-subtitle">Ancient incantations from the forgotten realms</p>
    </div>

    <div class="grimoire-wrapper">
      <div class="grimoire-decoration left"></div>

      <div class="grimoire-pages">
        <div class="controls-container">
          <div class="search-and-filters">
            <input v-model="filters.search" type="text" placeholder="Search spells..." class="filter-input" />
            <select v-model="filters.level" class="filter-select">
              <option value="">All Levels</option>
              <option v-for="lvl in 9" :key="lvl" :value="lvl">Level {{ lvl }}</option>
            </select>
            <select v-model="filters.school" class="filter-select">
              <option value="">All Schools</option>
              <option v-for="school in schools" :key="school" :value="school">{{ school }}</option>
            </select>
            <button @click="applyFilters" class="apply-filters-btn">Apply Filters</button>
          </div>

          <button @click="clearAllFilters" class="clear-filters-btn">
            <span class="filter-icon">🧹</span> Clear Filters
          </button>
        </div>

        <div class="spell-count" v-if="totalSpells">
          <span class="spell-count-icon">📜</span>
          <span>Showing {{ showingStart }}-{{ showingEnd }} of {{ totalSpells }} spells</span>
        </div>

        <div v-if="loading" class="loading-container">
          <div class="loading-spinner"></div>
          <p>Consulting the ancient tomes...</p>
        </div>

        <div v-else-if="error" class="error-message">
          <p>It seems that the spells are not within your reach, the grimoire needs a new chance.</p>
          <button @click="fetchPageData()" class="retry-button">Try Again</button>
        </div>

        <div v-else class="grid-container">
          <AgGridVue class="ag-theme-alpine dnd-theme" :columnDefs="columnDefs" :rowData="spells" :pagination="false"
            :domLayout="'normal'" :defaultColDef="defaultColDef" :getRowId="getRowId" @grid-ready="onGridReady"
            @filter-changed="onFilterChanged" @sort-changed="onSortChanged" :components="components" />
        </div>

        <div class="pagination-wrapper" v-if="totalSpells && totalPages > 1">
          <div class="pagination-controls">
            <button @click="goToPreviousPage" :disabled="currentPage === 1" class="page-button prev-button">
              <span class="page-arrow">◀</span> Previous
            </button>

            <div class="page-numbers">
              <button v-if="visiblePages[0] > 1" @click="goToPage(1)" class="page-number">1</button>
              <span v-if="visiblePages[0] > 2" class="page-ellipsis">...</span>

              <button v-for="page in visiblePages" :key="page"
                :class="['page-number', { active: page === currentPage }]" @click="goToPage(page)">
                {{ page }}
              </button>

              <span v-if="visiblePages[visiblePages.length - 1] < totalPages - 1" class="page-ellipsis">...</span>
              <button v-if="visiblePages[visiblePages.length - 1] < totalPages" @click="goToPage(totalPages)"
                class="page-number">
                {{ totalPages }}
              </button>
            </div>

            <button @click="goToNextPage" :disabled="currentPage === totalPages" class="page-button next-button">
              Next <span class="page-arrow">▶</span>
            </button>
          </div>
        </div>
      </div>

      <div class="grimoire-decoration right"></div>
    </div>

    <transition name="modal-fade">
      <div v-if="showModal && selectedSpell" class="modal-overlay" @click.self="closeModal">
        <div class="spell-scroll" :class="{ 'modal-entering': modalTransition }">
          <button @click="closeModal" class="modal-close-button">&times;</button>

          <div class="spell-seal"></div>
          <div class="spell-seal bottom"></div>

          <div class="spell-scroll-inner">
            <div class="spell-header">
              <h2 class="spell-title">{{ selectedSpell.name }}</h2>
              <div class="spell-subtitle">
                <span style="color: black;">{{ selectedSpell.level === 0 ? 'Cantrip' : `Level ${selectedSpell.level}`
                  }}</span>
                <span style="color: black;">{{ selectedSpell.school }}</span>
              </div>
            </div>

            <div class="spell-details">
              <div class="detail-row">
                <div class="detail-column">
                  <div class="detail-group">
                    <h3>Casting Time</h3>
                    <p style="color: black;">{{ selectedSpell.casting_time }}</p>
                  </div>

                  <div class="detail-group">
                    <h3>Range</h3>
                    <p style="color: black;">{{ selectedSpell.range }}</p>
                  </div>
                </div>

                <div class="detail-column">
                  <div class="detail-group">
                    <h3>Components</h3>
                    <p style="color: black;">{{ selectedSpell.components }}</p>
                    <p v-if="selectedSpell.material" class="material-components" style="color: black;">
                      ({{ selectedSpell.material }})
                    </p>
                  </div>

                  <div class="detail-group">
                    <h3>Duration</h3>
                    <p style="color: black;">{{ selectedSpell.duration }}</p>
                  </div>
                </div>
              </div>

              <div class="tag-container">
                <span v-if="selectedSpell.ritual" class="tag ritual">Ritual</span>
                <span v-if="selectedSpell.concentration" class="tag concentration">Concentration</span>
              </div>

              <div class="description-container">
                <h3>Description</h3>
                <div class="spell-description" v-html="selectedSpell.description"></div>

                <div v-if="selectedSpell.higher_level" class="higher-level">
                  <h3>At Higher Levels</h3>
                  <p>{{ selectedSpell.higher_level }}</p>
                </div>
              </div>
            </div>

            <div class="modal-navigation">
              <button @click="goToPreviousSpell" class="nav-button" :disabled="!hasPreviousSpell">
                <span>◀</span> Previous Spell
              </button>

              <button @click="goToNextSpell" class="nav-button" :disabled="!hasNextSpell">
                Next Spell <span>▶</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from 'vue';
import { AgGridVue } from 'ag-grid-vue3';
import 'ag-grid-community/styles/ag-grid.css';
import { useSpellStore } from '@/stores/spells';
import { storeToRefs } from 'pinia';
import { debounce } from 'lodash-es';

const store = useSpellStore();
const { spells, loading, error, totalSpells } = storeToRefs(store);

const gridApi = ref(null);
const gridColumnApi = ref(null);

const activeFilters = ref({});
const activeSorting = ref([]);

const columnDefs = ref([
  {
    field: 'name',
    headerName: 'Spell Name',
    filter: false, // Desativa o filtro padrão
    sortable: true,
    flex: 1,
    minWidth: 200,
    cellClass: 'spell-name-cell',
    cellRenderer: params => {
      return `<div class="spell-name">${params.value}</div>`;
    }
  },
  {
    field: 'level',
    headerName: 'Level',
    filter: false, // Desativa o filtro padrão
    sortable: true,
    width: 100,
  },
  {
    field: 'school',
    headerName: 'School',
    filter: false, // Desativa o filtro padrão
    sortable: true,
    width: 150,
    cellRenderer: params => {
      const schoolClass = params.value?.toLowerCase().replace(/\s/g, '-') || '';
      return `<div class="school-badge ${schoolClass}">${params.value}</div>`;
    }
  },
  {
    headerName: '',
    cellRenderer: 'buttonRenderer',
    width: 120,
    sortable: false,
    filter: false, // Desativa o filtro padrão
    cellRendererParams: {
      onClick: (data) => openModal(data),
    },
  },
]);

const buttonRenderer = (params) => {
  const button = document.createElement('button');
  button.innerHTML = '<span class="detail-icon">📖</span> Details';
  button.className = 'details-button';
  button.addEventListener('click', () => {
    params.onClick(params.data);
  });
  return button;
};

const components = {
  buttonRenderer,
};

const defaultColDef = {
  resizable: true,
  filter: true,
  sortable: true,
  suppressMovable: true,
  suppressMenu: true,
  suppressDragLeaveHidesColumns: true
};

const pageSize = ref(20);
const currentPage = ref(1);
const showingStart = computed(() => (currentPage.value - 1) * pageSize.value + 1);
const showingEnd = computed(() => Math.min(currentPage.value * pageSize.value, totalSpells.value || 0));

const totalPages = computed(() => {
  if (totalSpells.value && totalSpells.value > 0) {
    return Math.ceil(totalSpells.value / pageSize.value);
  }
  return 1;
});

const maxVisiblePages = 5;

const visiblePages = computed(() => {
  const pages = [];
  const halfVisible = Math.floor(maxVisiblePages / 2);
  let startPage = Math.max(1, currentPage.value - halfVisible);
  const endPage = Math.min(totalPages.value, startPage + maxVisiblePages - 1);

  if (endPage - startPage + 1 < maxVisiblePages) {
    startPage = Math.max(1, endPage - maxVisiblePages + 1);
  }

  for (let i = startPage; i <= endPage; i++) {
    pages.push(i);
  }

  return pages;
});

const fetchPageData = async (filterModel = {}, sortModel = []) => {
  if (loading.value) return;

  loading.value = true;
  try {
    await store.fetchSpells(currentPage.value, pageSize.value, filterModel, sortModel);

    if (gridApi.value) {
      gridApi.value.setRowData(spells.value);
      gridApi.value.sizeColumnsToFit();
    }
  } catch (err) {
    console.error('Error fetching spell data:', err);
  } finally {
    loading.value = false;
  }
};

const onFilterChanged = debounce(async () => {
  if (!gridApi.value) return;
  const filterModel = gridApi.value.getFilterModel();
  activeFilters.value = filterModel;
  currentPage.value = 1;
  await fetchPageData(filterModel, activeSorting.value);
}, 300);

const onSortChanged = debounce(async () => {
  if (!gridApi.value) return;
  const sortModel = gridApi.value.getSortModel();
  activeSorting.value = sortModel;
  await fetchPageData(activeFilters.value, sortModel);
}, 300);

const onGridReady = async (params) => {
  gridApi.value = params.api;
  gridColumnApi.value = params.columnApi;

  const gridDiv = document.querySelector('.ag-theme-alpine');
  if (gridDiv) {
    gridDiv.classList.add('dnd-theme');
  }

  params.api.sizeColumnsToFit();

  window.addEventListener('resize', () => {
    params.api.sizeColumnsToFit();
  });
};

const goToPreviousPage = async () => {
  if (currentPage.value > 1) {
    currentPage.value--;
    await fetchPageData(activeFilters.value, activeSorting.value);
  }
};

const goToNextPage = async () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
    await fetchPageData(activeFilters.value, activeSorting.value);
  }
};

const goToPage = async (page) => {
  if (page !== currentPage.value) {
    currentPage.value = page;
    await fetchPageData(activeFilters.value, activeSorting.value);
  }
};

const getRowId = (params) => params.data.id;

const clearAllFilters = () => {
  if (gridApi.value) {
    gridApi.value.setFilterModel(null);
    onFilterChanged();
  }
};

const filters = ref({
  search: '',
  level: '',
  school: ''
});

const schools = ref([
  'Abjuration',
  'Conjuration',
  'Divination',
  'Enchantment',
  'Evocation',
  'Illusion',
  'Necromancy',
  'Transmutation'
]);

const applyFilters = async () => {
  currentPage.value = 1; // Reinicia para a primeira página
  await fetchPageData(filters.value);
};

const showModal = ref(false);
const selectedSpell = ref(null);
const modalTransition = ref(false);

const openModal = (spell) => {
  selectedSpell.value = spell;
  modalTransition.value = true;
  setTimeout(() => {
    showModal.value = true;
  }, 50);
};

const closeModal = () => {
  showModal.value = false;
  setTimeout(() => {
    selectedSpell.value = null;
    modalTransition.value = false;
  }, 300);
};

const currentSpellIndex = computed(() => {
  if (!selectedSpell.value) return -1;
  return spells.value.findIndex(spell => spell.id === selectedSpell.value.id);
});

const hasPreviousSpell = computed(() => {
  return currentSpellIndex.value > 0;
});

const hasNextSpell = computed(() => {
  return currentSpellIndex.value < spells.value.length - 1 && currentSpellIndex.value !== -1;
});

const goToPreviousSpell = () => {
  if (hasPreviousSpell.value) {
    selectedSpell.value = spells.value[currentSpellIndex.value - 1];
  }
};

const goToNextSpell = () => {
  if (hasNextSpell.value) {
    selectedSpell.value = spells.value[currentSpellIndex.value + 1];
  }
};

const handleKeyPress = (e) => {
  if (!showModal.value) return;

  if (e.key === 'Escape') {
    closeModal();
  } else if (e.key === 'ArrowLeft' && hasPreviousSpell.value) {
    goToPreviousSpell();
  } else if (e.key === 'ArrowRight' && hasNextSpell.value) {
    goToNextSpell();
  }
};

onMounted(async () => {
  document.addEventListener('keydown', handleKeyPress);
  await fetchPageData();
});

watch(() => selectedSpell.value, () => {
  if (selectedSpell.value) {
    document.body.style.overflow = 'hidden';
  } else {
    document.body.style.overflow = '';
  }
});
</script>

<style scoped>
.spells-container {
  width: 100%;
  font-family: 'Cinzel', serif;
  color: var(--color-text);
  margin: 0 auto;
  padding: 20px 0;
}

.spells-header {
  text-align: center;
  margin-bottom: 30px;
  position: relative;
}

.spells-header:after {
  content: "";
  position: absolute;
  bottom: -15px;
  left: 50%;
  transform: translateX(-50%);
  width: 150px;
  height: 4px;
  background: linear-gradient(90deg, transparent, var(--color-primary), transparent);
  box-shadow: 0 0 10px var(--color-primary);
}

.spells-title {
  font-size: 2.8rem;
  font-weight: 700;
  margin: 0;
  color: var(--color-primary);
  text-shadow: 1px 1px 3px var(--shadow-color);
  letter-spacing: 3px;
}

.spells-subtitle {
  font-size: 1.2rem;
  margin-top: 10px;
  font-style: italic;
  opacity: 0.85;
}

.grimoire-wrapper {
  display: flex;
  width: 100%;
  position: relative;
  min-height: 75vh;
  margin: 20px auto;
  padding: 0;
}

.grimoire-decoration {
  width: 50px;
  position: relative;
  background-repeat: no-repeat;
  background-size: cover;
}

.grimoire-decoration.left {
  border-radius: 20px 0 0 20px;
  background: linear-gradient(to right, var(--color-accent), var(--header-bg));
  box-shadow: inset -5px 0 10px rgba(0, 0, 0, 0.3);
  position: relative;
}

.grimoire-decoration.right {
  border-radius: 0 20px 20px 0;
  background: linear-gradient(to left, var(--color-accent), var(--header-bg));
  box-shadow: inset 5px 0 10px rgba(0, 0, 0, 0.3);
}

.grimoire-decoration.left::before,
.grimoire-decoration.right::before {
  content: "";
  position: absolute;
  top: 50%;
  width: 20px;
  height: 20px;
  background-color: var(--color-secondary);
  border-radius: 50%;
  box-shadow: 0 0 10px var(--color-primary);
}

.grimoire-decoration.left::before {
  right: 10px;
  transform: translateY(-50%);
}

.grimoire-decoration.right::before {
  left: 10px;
  transform: translateY(-50%);
}

.grimoire-decoration.left::after,
.grimoire-decoration.right::after {
  content: "";
  position: absolute;
  height: 80%;
  width: 5px;
  top: 10%;
  background: linear-gradient(to bottom, var(--color-border), transparent, var(--color-border));
}

.grimoire-decoration.left::after {
  right: 5px;
}

.grimoire-decoration.right::after {
  left: 5px;
}

.grimoire-pages {
  flex: 1;
  background-color: var(--color-background);
  padding: 30px;
  border-top: 5px double var(--color-border);
  border-bottom: 5px double var(--color-border);
  position: relative;
  box-shadow: inset 0 0 30px rgba(0, 0, 0, 0.2);
  background-image:
    linear-gradient(rgba(255, 255, 255, 0.05) 1px, transparent 1px),
    linear-gradient(90deg, rgba(255, 255, 255, 0.05) 1px, transparent 1px);
  background-size: 20px 20px;
  display: flex;
  flex-direction: column;
}

.grimoire-pages::before {
  content: '';
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  width: 30px;
  background: linear-gradient(to right, rgba(0, 0, 0, 0.1), transparent);
}

.grimoire-pages::after {
  content: '';
  position: absolute;
  top: 0;
  bottom: 0;
  right: 0;
  width: 30px;
  background: linear-gradient(to left, rgba(0, 0, 0, 0.1), transparent);
}

.controls-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  padding: 0 0 15px 0;
  border-bottom: 2px solid var(--color-secondary);
}

.search-and-filters {
  display: flex;
  gap: 15px;
  align-items: center;
}

.filter-input {
  padding: 8px;
  border: 1px solid var(--color-border);
  border-radius: 4px;
  font-family: 'Cinzel', serif;
  font-size: 0.9rem;
  width: 200px;
}

.filter-select {
  padding: 8px;
  border: 1px solid var(--color-border);
  border-radius: 4px;
  font-family: 'Cinzel', serif;
  font-size: 0.9rem;
}

.apply-filters-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  background-color: var(--color-secondary);
  color: var(--color-background);
  border: 1px solid var(--color-border);
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-family: 'Cinzel', serif;
  font-weight: 600;
  letter-spacing: 1px;
  transition: all 0.3s ease;
  box-shadow: 0 2px 5px var(--shadow-color);
  width: auto;
}

.apply-filters-btn:hover {
  background-color: var(--color-primary);
  transform: translateY(-2px);
  box-shadow: 0 4px 8px var(--shadow-color);
}

.clear-filters-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  background-color: var(--color-secondary);
  color: var(--color-background);
  border: 1px solid var(--color-border);
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-family: 'Cinzel', serif;
  font-weight: 600;
  letter-spacing: 1px;
  transition: all 0.3s ease;
  box-shadow: 0 2px 5px var(--shadow-color);
}

.clear-filters-btn:hover {
  background-color: var(--color-primary);
  transform: translateY(-2px);
  box-shadow: 0 4px 8px var(--shadow-color);
}

.filter-icon,
.spell-count-icon {
  font-size: 16px;
}

.spell-count {
  font-size: 0.9rem;
  opacity: 0.9;
  display: flex;
  align-items: center;
  gap: 8px;
  font-style: italic;
}

.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 300px;
  gap: 20px;
}

.loading-spinner {
  width: 60px;
  height: 60px;
  border: 4px solid rgba(0, 0, 0, 0.1);
  border-left-color: var(--color-primary);
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.error-message {
  background-color: rgba(139, 0, 0, 0.1);
  border: 2px solid var(--color-primary);
  padding: 20px;
  border-radius: 8px;
  text-align: center;
  margin: 30px 0;
}

.retry-button {
  margin-top: 15px;
  background-color: var(--color-primary);
  color: var(--color-background);
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
  font-family: 'Cinzel', serif;
  transition: all 0.3s ease;
}

.retry-button:hover {
  background-color: #a00;
  transform: translateY(-2px);
}

.grid-container {
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  height: 60vh;
  /* Reduced height to make room for pagination */
  border: 3px solid var(--color-accent);
  border-radius: 5px;
  overflow: hidden;
  background-color: var(--color-background);
  color: var(--color-text);
}

:deep(.ag-theme-alpine) {
  height: 100%;
  width: 100%;
}

:deep(.dnd-theme) {
  --ag-background-color: var(--color-background);
  --ag-odd-row-background-color: rgba(0, 0, 0, 0.05);
  --ag-header-background-color: var(--color-accent);
  --ag-header-foreground-color: var(--color-background);
  --ag-border-color: var(--color-accent);
  --ag-row-border-color: var(--color-accent);
  --ag-row-hover-color: rgba(139, 0, 0, 0.1);
  --ag-selected-row-background-color: rgba(139, 0, 0, 0.15);
  --ag-font-size: 14px;
  --ag-font-family: 'Cinzel', serif;
  height: 100%;
  width: 100%;
}

:deep(.ag-header) {
  border-bottom: 2px solid var(--color-primary);
  font-weight: bold;
  text-transform: uppercase;
  letter-spacing: 1px;
}

:deep(.ag-row) {
  border-bottom: 1px solid rgba(0, 0, 0, 0.1);
  transition: background-color 0.2s ease;
}

:deep(.ag-row:hover) {
  box-shadow: inset 0 0 0 1px var(--color-secondary);
}

:deep(.ag-cell) {
  padding: 12px 15px;
  display: flex;
  align-items: center;
  color: var(--color-text);
}

:deep(.spell-name) {
  font-weight: 600;
  color: var(--color-primary);
}

:deep(.cantrip-badge) {
  background-color: #6a7d8b;
  color: white;
  padding: 2px 8px;
  border-radius: 10px;
  font-size: 12px;
  display: inline-block;
}

:deep(.school-badge) {
  padding: 3px 8px;
  border-radius: 4px;
  font-size: 12px;
  background-color: var(--color-secondary);
  color: var(--color-background);
}

:deep(.concentration-badge) {
  background-color: #8b0000;
  color: white;
  padding: 2px 8px;
  border-radius: 10px;
  font-size: 12px;
}

:deep(.details-button) {
  background-color: var(--color-secondary);
  color: var(--color-background);
  border: 1px solid var(--color-border);
  border-radius: 4px;
  padding: 5px 10px;
  font-family: 'Cinzel', serif;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 5px;
  justify-content: center;
}

:deep(.details-button:hover) {
  background-color: var(--color-primary);
  transform: translateY(-2px);
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
}

:deep(.detail-icon) {
  font-size: 14px;
}

.pagination-wrapper {
  margin-top: 20px;
  padding-top: 15px;
  border-top: 2px solid var(--color-secondary);
  display: block;
  /* Ensure it's visible */
}

.pagination-controls {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 15px;
}

.page-button {
  background-color: var(--color-secondary);
  color: var(--color-background);
  border: none;
  padding: 8px 15px;
  border-radius: 4px;
  cursor: pointer;
  font-family: 'Cinzel', serif;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
  box-shadow: 0 2px 4px var(--shadow-color);
}

.page-button:hover:not(:disabled) {
  background-color: var(--color-primary);
  transform: translateY(-2px);
}

.page-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-numbers {
  display: flex;
  gap: 8px;
  align-items: center;
}

.page-number {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: transparent;
  color: var(--color-text);
  border: 1px solid var(--color-accent);
  border-radius: 4px;
  cursor: pointer;
  font-family: 'Cinzel', serif;
  transition: all 0.3s ease;
}

.page-number:hover:not(.active) {
  background-color: var(--color-secondary);
  color: var(--color-background);
}

.page-number.active {
  background-color: var(--color-primary);
  color: var(--color-background);
  border-color: var(--color-primary);
  box-shadow: 0 0 5px var(--color-primary);
}

.page-ellipsis {
  font-size: 16px;
  opacity: 0.7;
}

.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.3s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.75);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  backdrop-filter: blur(3px);
  animation: fadeIn 0.3s ease-out;
}

@keyframes fadeIn {
  from {
    background-color: rgba(0, 0, 0, 0);
    backdrop-filter: blur(0px);
  }

  to {
    background-color: rgba(0, 0, 0, 0.75);
    backdrop-filter: blur(3px);
  }
}

.spell-scroll {
  background-color: var(--color-background);
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='100' height='100' opacity='0.05'%3E%3Cpath d='M11 18c3.866 0 7-3.134 7-7s-3.134-7-7-7-7 3.134-7 7 3.134 7 7 7zm48 25c3.866 0 7-3.134 7-7s-3.134-7-7-7-7 3.134-7 7 3.134 7 7 7zm-43-7c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zm63 31c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zM34 90c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zm56-76c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zM12 86c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm28-65c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm23-11c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm-6 60c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm29 22c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zM32 63c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm57-13c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm-9-21c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM60 91c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM35 41c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM12 60c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2z' fill='%23000000' fill-opacity='0.1' fill-rule='evenodd'/%3E%3C/svg%3E"),
    linear-gradient(to bottom,
      rgba(255, 248, 220, 0.9),
      rgba(255, 248, 220, 0.95) 30%,
      rgba(255, 248, 220, 0.9));
  max-width: 800px;
  width: 90%;
  max-height: 85vh;
  position: relative;
  overflow-y: auto;
  box-shadow: 0 5px 35px rgba(0, 0, 0, 0.65);
  border: none;
  border-radius: 0;
  padding: 0;
  transform: translateY(20px);
  animation: slideUp 0.4s ease-out forwards;
}

.modal-entering {
  opacity: 0;
  transform: translateY(50px);
}

@keyframes slideUp {
  from {
    transform: translateY(30px);
    opacity: 0.5;
  }

  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.spell-scroll::before,
.spell-scroll::after {
  content: "";
  position: absolute;
  left: 0;
  right: 0;
  height: 30px;
  background-size: 30px 30px;
}

.spell-scroll::before {
  top: 0;
  background-image: radial-gradient(circle at 15px 15px,
      rgba(139, 0, 0, 0.4) 0%,
      rgba(139, 0, 0, 0.1) 50%,
      transparent 70%);
}

.spell-scroll::after {
  bottom: 0;
  background-image: radial-gradient(circle at 15px 15px,
      rgba(139, 0, 0, 0.4) 0%,
      rgba(139, 0, 0, 0.1) 50%,
      transparent 70%);
}

.spell-scroll-inner {
  padding: 40px 50px;
  position: relative;
  z-index: 1;
}

.spell-seal {
  position: absolute;
  width: 80px;
  height: 80px;
  top: 20px;
  right: 20px;
  background-color: rgba(139, 0, 0, 0.1);
  border: 2px solid var(--color-primary);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  opacity: 0.8;
}

.spell-seal::before {
  content: "";
  position: absolute;
  width: 70%;
  height: 70%;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100' fill='none' stroke='%238b0000' stroke-width='2'%3E%3Ccircle cx='50' cy='50' r='35'/%3E%3Cpath d='M50 15v70M15 50h70M29.29 29.29l41.42 41.42M29.29 70.71l41.42-41.42M50 15l20 35L50 85 30 50z'/%3E%3C/svg%3E");
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
}

.spell-seal.bottom {
  top: auto;
  bottom: 20px;
  left: 20px;
  right: auto;
  width: 60px;
  height: 60px;
  opacity: 0.6;
}

.spell-header {
  text-align: center;
  margin-bottom: 25px;
  padding-bottom: 15px;
  position: relative;
  padding-top: 30px;
}

.spell-header::after {
  content: "";
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 70%;
  height: 3px;
  background: linear-gradient(90deg, transparent, var(--color-primary), transparent);
}

.spell-title {
  font-size: 2.2rem;
  color: var(--color-primary);
  margin: 0 0 10px;
  text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.15);
  letter-spacing: 2px;
  font-weight: 700;
}

.spell-subtitle {
  display: flex;
  justify-content: center;
  gap: 15px;
  font-style: italic;
  margin-top: 5px;
}

.spell-details {
  padding: 0 20px;
}

.detail-row {
  display: flex;
  margin-bottom: 20px;
  gap: 30px;
  border-bottom: 1px solid rgba(139, 0, 0, 0.2);
  padding-bottom: 15px;
}

.detail-column {
  flex: 1;
}

.detail-group {
  margin-bottom: 15px;
}

.detail-group h3 {
  font-size: 1.1rem;
  color: var(--color-secondary);
  margin: 0 0 5px;
  font-weight: 600;
  letter-spacing: 1px;
}

.detail-group p {
  margin: 0;
  line-height: 1.5;
}

.material-components {
  font-size: 0.9rem;
  margin-top: 5px;
  color: var(--color-text-light);
}

.tag-container {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.tag {
  padding: 4px 10px;
  border-radius: 12px;
  font-size: 0.85rem;
  font-weight: 500;
  letter-spacing: 0.5px;
}

.tag.ritual {
  background-color: #4b5e83;
  color: white;
}

.tag.concentration {
  background-color: var(--color-primary);
  color: white;
}

.description-container {
  margin-top: 20px;
}

.description-container h3 {
  font-size: 1.2rem;
  color: var(--color-secondary);
  margin: 0 0 10px;
  font-weight: 600;
}

.spell-description {
  color: #000000;
  background-color: rgba(255, 255, 255, 0.4);
  padding: 15px;
  border-left: 3px solid var(--color-secondary);
  line-height: 1.6;
  margin-bottom: 20px;
  text-align: justify;
}

.higher-level {
  background-color: rgba(139, 0, 0, 0.05);
  padding: 15px;
  border-left: 3px solid var(--color-primary);
  margin-top: 20px;
}

.higher-level h3 {
  color: var(--color-primary);
  margin: 0 0 10px;
}

.higher-level p {
  margin: 0;
  line-height: 1.6;
}

.modal-navigation {
  display: flex;
  justify-content: space-between;
  margin-top: 30px;
  padding: 20px 20px 40px;
}

.nav-button {
  background-color: transparent;
  color: var(--color-secondary);
  border: 2px solid var(--color-secondary);
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-family: 'Cinzel', serif;
  font-weight: 600;
  letter-spacing: 1px;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
}

.nav-button:hover:not(:disabled) {
  background-color: var(--color-secondary);
  color: var(--color-background);
  transform: translateY(-2px);
}

.nav-button:disabled {
  opacity: 0.4;
  cursor: not-allowed;
  border-color: #999;
  color: #999;
}

.modal-close-button {
  position: absolute;
  top: 15px;
  right: 15px;
  background-color: transparent;
  border: 2px solid var(--color-secondary);
  color: var(--color-secondary);
  font-size: 1.5rem;
  width: 32px;
  height: 32px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s ease;
  z-index: 10;
  padding: 0;
  line-height: 0;
}

.modal-close-button:hover {
  background-color: var(--color-secondary);
  color: var(--color-background);
  transform: rotate(90deg);
}

.spell-scroll::-webkit-scrollbar {
  width: 12px;
}

.spell-scroll::-webkit-scrollbar-track {
  background: rgba(255, 248, 220, 0.5);
}

.spell-scroll::-webkit-scrollbar-thumb {
  background-color: var(--color-secondary);
  border-radius: 6px;
  border: 3px solid rgba(255, 248, 220, 0.8);
}

.school-badge.abjuration {
  background-color: #1e88e5;
}

.school-badge.conjuration {
  background-color: #7cb342;
}

.school-badge.divination {
  background-color: #9c27b0;
}

.school-badge.enchantment {
  background-color: #ec407a;
}

.school-badge.evocation {
  background-color: #f57c00;
}

.school-badge.illusion {
  background-color: #00bcd4;
}

.school-badge.necromancy {
  background-color: #424242;
}

.school-badge.transmutation {
  background-color: #fbc02d;
}
</style>