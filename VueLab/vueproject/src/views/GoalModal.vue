<template>
  <div class="modal-backdrop">
    <div class="modal">
      <h2>{{ isEditing ? 'Редактировать цель' : 'Создать цель' }}</h2>

      <label>Название</label>
      <input v-model="goal.title" type="text" />

      <label>Описание</label>
      <textarea v-model="goal.description" />

      <label>Дедлайн</label>
      <input v-model="goal.deadline" type="date" />

      <label>Статус</label>
      <select v-model="goal.statusId">
        <option v-for="status in statuses" :key="status.id" :value="status.id">
          {{ status.name }}
        </option>
      </select>

      <label>Приоритет</label>
      <select v-model="goal.priorityId">
        <option v-for="priority in importances" :key="priority.id" :value="priority.id">
          {{ priority.name }}
        </option>
      </select>

      <div class="modal-actions">
        <button @click="save">Сохранить</button>
        <button @click="$emit('close')">Отмена</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, watch, computed } from 'vue';
import { toRaw, toRefs } from 'vue';

const props = defineProps({
  modelValue: Boolean,
  goalData: Object,
  statuses: Array,
  importances: Array,
  themes: Array,
  users: Array
});

const emit = defineEmits(['update:modelValue', 'save', 'close']);

const isEditing = computed(() => !!props.goalData?.id);


// Локальная копия цели
const goal = reactive({
  id: null,
  title: '',
  description: '',
  deadline: '',
  statusId: null,
  priorityId: null,
  themeId: null,
  userId: null,
});

watch(
  () => props.goalData,
  (newGoal) => {
    if (newGoal) {
      Object.assign(goal, newGoal);
    } else {
      Object.assign(goal, {
        id: null,
        title: '',
        description: '',
        deadline: '',
        statusId: null,
        priorityId: null,
        themeId: null,
        userId: null,
      });
    }
  },
  { immediate: true }
);

function save() {
  emit('save', toRaw(goal));
  emit('update:modelValue', false);
}
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  inset: 0;
  background-color: var(--color-background-soft);
  display: flex;
  align-items: center;
  justify-content: center;
}
.modal {
  background: var(--color-accent);
  padding: 20px;
  border-radius: 8px;
  width: 400px;
}
.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 1rem;
}
</style>
