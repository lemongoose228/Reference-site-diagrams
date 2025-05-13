import { createRouter, createWebHistory } from 'vue-router';
import LoginPage from '../components/LoginPage.vue';
import MainTeacher from '../components/MainTeacher.vue';
import MainStudent from '../components/MainStudent.vue';
import CreateArticle from '../components/CreateArticle.vue';
import EditArticle from '../components/EditArticle.vue';
import Article from '../components/Article.vue';

const routes = [
    {
        path: '/',
        name: 'Login',
        component: LoginPage
    },
    {
        path: '/teacher',
        name: 'Teacher',
        component: MainTeacher
    },
    {
        path: '/student',
        name: 'Student',
        component: MainStudent,
    },
    {
        path: '/article/create',
        name: 'CreateArticle',
        component: CreateArticle,
    },
    {
        path: '/article/edit',
        name: 'EditArticle',
        component: EditArticle,
    },
    {
        path: '/article',
        name: 'Article',
        component: Article,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;

