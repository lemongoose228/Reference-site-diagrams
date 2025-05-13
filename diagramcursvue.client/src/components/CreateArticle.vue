<template>
	<div class="container">
		<header>
			<router-link to="/teacher">
				<span class="nav">Выйти</span>
			</router-link>
		</header>
		<h1 class="title main_title">Создание статьи</h1>

		<form @submit="submitArticle">
			<h2 class="title">Добавьте теги:</h2>
			<div class="filter-buttons">
				<button type="button" class="filter-button" data-category="time" data-color="active-blue">
					<input v-model="tags" class="filter_checkbox" type="checkbox" value="time">
					Измерение во времени
				</button>
				<button type="button" class="filter-button" data-category="part" data-color="active-yellow">
					<input v-model="tags" class="filter_checkbox" type="checkbox" value="part">
					Целое на части
				</button>
				<button type="button" class="filter-button" data-category="compare" data-color="active-pink">
					<input v-model="tags" class="filter_checkbox" type="checkbox" value="compare">
					Сравнение
				</button>
				<button type="button" class="filter-button" data-category="relation" data-color="active-orange">
					<input v-model="tags" class="filter_checkbox" type="checkbox" value="relation">
					Отношения
				</button>
			</div>

			<h2 class="title">Краткое описание:</h2>
			<div class="editor_wrapper">
				<div class="editor" id="short_description_editor"></div>
				<div id="short_description_view" class="view_area"></div>
			</div>

			<h2 class="title">Текст статьи:</h2>
			<div class="editor_wrapper">
				<div class="editor" id="article_text_editor"></div>
				<div id="article_text_view" class="view_area"></div>
			</div>

			<div class="checkbox_wrapper">
				<h2 class="title">Опубликовать:</h2>
				<input v-model="publish_checkbox" class="publish_checkbox" type="checkbox" name="" id="">
			</div>

			<div class="edit_article_submit_wrapper">
				<button class="edit_article_submit" type="submit">Сохранить</button>
			</div>

		</form>
	</div>
		
</template>

<script lang="js">
		import { defineComponent } from 'vue';

		export default defineComponent({
				data() {
						return {
								publish_checkbox: false,
								tags: [],
								mirrorShortDescription: null,
								mirrorArticleText: null,

						}
				},
				created() {
				},
				async mounted() {
						if (!sessionStorage.getItem('isAdmin')) {
								this.$router.push('/');
						}

						const short_description_view = document.getElementById("short_description_view");
						const short_description_editor = document.getElementById("short_description_editor");

						const article_text_view = document.getElementById("article_text_view");
						const article_text_editor = document.getElementById("article_text_editor");
						
						
						let short_description_value; 
						if (localStorage.getItem("short_description")) {
								short_description_value = localStorage.getItem("short_description")
						}
						else {
								short_description_value = "# Hello!\n\nThis is a short description editor.";
						}

						const mirrorShortDescription = CodeMirror(short_description_editor, {
								value: short_description_value,
								mode: "markdown",
								theme: "material",
								lineNumbers: true,
								autoCloseBrackets: true,
								styleActiveLine: true,
								lineWrapping: true,
						});

						this.mirrorShortDescription = mirrorShortDescription;
						
						let markdownText = mirrorShortDescription.getValue();
						let html = marked.parse(markdownText);
						short_description_view.innerHTML = html;
						
						mirrorShortDescription.on("change", function(cm, changeObj) {
								const markdownText = cm.getValue();
								const html = marked.parse(markdownText);
								short_description_view.innerHTML = html;
								localStorage.setItem("short_description", markdownText);
						});
						
						
						let article_text_value; 
						if (localStorage.getItem("article_text")) {
								article_text_value = localStorage.getItem("article_text")
						}
						else {
								article_text_value = "# Hello!\n\nThis is a article text editor.";
						}
						
						const mirrorArticleText = CodeMirror(article_text_editor, {
								value: article_text_value,
								mode: "markdown",
								theme: "material",
								lineNumbers: true,
								autoCloseBrackets: true,
								styleActiveLine: true,
								lineWrapping: true,
						});
						
						this.mirrorArticleText = mirrorArticleText;


						markdownText = mirrorArticleText.getValue();
						html = marked.parse(markdownText);
						article_text_view.innerHTML = html;

						mirrorArticleText.on("change", function(cm, changeObj) {
								const markdownText = cm.getValue();
								const html = marked.parse(markdownText);
								article_text_view.innerHTML = html;
								localStorage.setItem("article_text", markdownText);
						});

						const filterButtons = document.querySelectorAll('.filter-button');
						const activeCategories = new Set();
						filterButtons.forEach(button => {
								button.addEventListener('click', () => {
										const category = button.dataset.category;
										const colorClass = button.dataset.color;
						
										if (activeCategories.has(category)) {
												activeCategories.delete(category);
												button.classList.remove(colorClass);
												button.style.backgroundColor = '#555';
										} else {
												activeCategories.add(category);
												button.classList.add(colorClass);
												button.style.backgroundColor = '';
										}
						
								});
						});
				},
				methods: {
						async submitArticle(event) {
								event.preventDefault();

								const url = 'https://localhost:48867/api/create-article';
								const body = JSON.stringify({ 
										shortDescription: this.mirrorShortDescription.getValue(),
										articleText: this.mirrorArticleText.getValue(),
										tags: this.tags,
										publish: this.publish_checkbox
								});

								const res = await fetch(url, {
										method: 'POST',
										headers: {
												'Content-Type': 'application/json'
										},
										body: body
								});
						
								try {
										if (!res.ok) { 
												throw new Error(`Ошибка сервера: ${res.status}`);
										}

										localStorage.removeItem("short_description");
										localStorage.removeItem("article_text");
						
										console.log("Всё ок");

										this.$router.push('/teacher');
						
								} catch (error) {
										const errorText = await res.text();
										throw new Error(`HTTP Error: ${res.status} - ${res.statusText}, Body: ${errorText}`);
								}
						}
				}
		});
</script>


<style scoped>
		.container {
				max-width: 1200px;
				margin: 0 auto;
				padding: 20px;
		}

		.filter-buttons {
				display: flex;
				justify-content: center;
				margin-bottom: 20px;
		}

		.filter-button {
				padding: 10px 20px;
				margin: 0 10px;
				border: none;
				border-radius: 20px;
				font-size: 16px;
				cursor: pointer;
				background-color: #555;
				color: #fff;
				position: relative;
		}

		.filter-button.active-blue {
				background-color: skyblue;
				color: #000;
		}

		.filter-button.active-yellow {
				background-color: yellow;
				color: #000;
		}

		.filter-button.active-pink {
				background-color: deeppink;
				color: #fff;
		}

		.filter-button.active-orange {
				background-color: orange;
				color: #000;
		}

		.card-container {
				display: flex;
				flex-wrap: wrap;
				justify-content: center;
		}

		.card {
				width: 300px;
				margin-bottom: 20px;
				border-radius: 10px;
				overflow: hidden;
				background-color: #333;
				box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
				margin-left: 10px;  /* Added left margin */
				margin-right: 10px; /* Added right margin */
		}

		.card img {
				width: 100%;
				height: 200px;
				object-fit: cover;
		}

		.card-content {
				padding: 15px;
		}

		.card-title {
				font-size: 1.2em;
				margin-bottom: 10px;
		}

		.card-description {
				font-size: 0.9em;
				color: #ddd;
		}

		.category-indicators {
				display: flex;
				padding: 5px;
		}

		.category-indicator {
				width: 12px;
				height: 12px;
				border-radius: 50%;
				margin-right: 5px;
		}

		.category-indicator.time {
				background-color: skyblue;
		}

		.category-indicator.part {
				background-color: yellow;
		}

		.category-indicator.compare {
				background-color: deeppink;
		}

		.category-indicator.relation {
				background-color: orange;
		}

		header {
				display: flex;
				flex-direction: row;
				/* justify-content: space-around; */
		}

  .nav {
    padding: 10px 20px;
    border-radius: 20px;
    border: 1px solid #888; /* Default inactive border */
    background-color: transparent; /* Default inactive background */
    color: #fff;
    cursor: pointer;
    transition: all 0.3s ease; /* Smooth transition for hover effect */
    font-size: 16px;
  }

    .nav:hover {
      background-color: #9966FF; /* Active background color (Purple) */
      border-color: #9966FF; /* Active border color (Purple) */
      color: #fff;
    }

		.editor_wrapper {
				background-color: #fff;
				margin-bottom: 36px;
				display: flex;
				flex-direction: row;
				width: 100%;
				min-height: 300px;
		}

		.editor {
				width: 50%;
				resize: horizontal;
		}

		.title {
				color: #fff;
		}

		.checkbox_wrapper {
				display: flex;
				flex-direction: row;
				gap: 12px;
		}

		.publish_checkbox {
				width: 32px;
		}

		.edit_article_submit {
				padding: 14px 24px;
				border-radius: 20px;
				border: 1px solid #888; /* Default inactive border */
				background-color: transparent; /* Default inactive background */
				color: #fff;
				cursor: pointer;
				transition: all 0.3s ease; /* Smooth transition for hover effect */
				font-size: 18px;
				margin-top: 16px;
				margin-bottom: 32px;
		}

		.edit_article_submit:hover {
				background-color: #9966FF; /* Active background color (Purple) */
				border-color: #9966FF; /* Active border color (Purple) */
				color: #fff;
		}

		.edit_article_submit_wrapper {
				display: flex;
				flex-direction: row;
				justify-content: end;
		}

		.main_title {
				text-align: center;
		}

		.filter_checkbox {
				position: absolute;
				width: 100%;
				height: 100%;
				opacity: 0;
				cursor: pointer;
				left: 0px;
				top: 0px;
		}

		.article * {
		color: #fff;
		}

		.input_area {
				width: 50%;
		}

		.view_area {
				width: 50%;
				border-left: 3px solid black;
				padding: 8px 16px;

				overflow: scroll;
		}

		.view_area img {
				max-width: 500px;
				max-height: 500px;
		}

		.CodeMirror {
				/* border: 1px solid #eee; */
				height: 100%;
		}
</style>
