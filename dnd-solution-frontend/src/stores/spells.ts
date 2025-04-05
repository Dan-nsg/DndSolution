import { defineStore } from "pinia";
import axios from "axios";

interface Spell {
  id: string;
  index: string;
  name: string;
  description: string;
  higherLevel: string;
  range: string;
  components: string;
  material: string;
  ritual: boolean;
  duration: string;
  concentration: boolean;
  castingTime: string;
  level: number;
  school: string;
  classes: string;
  subclasses: string;
  url: string;
  createdAt: string;
  updatedAt: string | null;
}

interface SpellState {
  spells: Spell[];
  loading: boolean;
  error: string | null;
  totalSpells: number | null;
}

export const useSpellStore = defineStore("spells", {
  state: (): SpellState => ({
    spells: [],
    loading: false,
    error: null,
    totalSpells: null,
  }),
  actions: {
    async fetchSpells(page = 1, pageSize = 20) {
      this.loading = true;
      this.error = null;
      try {
        const response = await axios.get<{ data: Spell[]; total: number }>(
          `https://localhost:7051/api/spells?page=${page}&pageSize=${pageSize}`
        );
        this.spells = response.data.data;
        this.totalSpells = response.data.total;
      } catch (error: any) {
        this.error = error.message;
        console.error("Erro ao buscar spells:", error);
      } finally {
        this.loading = false;
      }
    },
    async syncSpells() {
      this.loading = true;
      try {
        await axios.post("https://localhost:7051/api/spells/sync");
        await this.fetchSpells();
      } catch (error: any) {
        this.error = error.message;
        console.error("Erro ao sincronizar spells:", error);
      } finally {
        this.loading = false;
      }
    },
  },
  getters: {
    totalPages: (state) => {
      if (state.totalSpells && state.totalSpells > 0) {
        return Math.ceil(state.totalSpells / 20);
      }
      return 0;
    },
  },
});
