/**
 * This configuration was generated using the CKEditor 5 Builder. You can modify it anytime using this link:
 * https://ckeditor.com/ckeditor-5/builder/?redirect=portal#installation/NoNgNARATAdAzPCkCMAGKyCszkHZkAsuIqupAHOSFlFKqgJyO51Sa7v4lIQCmAdklRhgyMMOFjJAXUgAzEHF5wAJnAjSgA==
 */

const {
	ClassicEditor,
	Alignment,
	AutoLink,
	Autosave,
	Bold,
	Essentials,
	ImageEditing,
	ImageUtils,
	Italic,
	Link,
	List,
	Paragraph,
	Underline
} = window.CKEDITOR;

const LICENSE_KEY =
	'eyJhbGciOiJFUzI1NiJ9.eyJleHAiOjE3NzUwMDE1OTksImp0aSI6ImI4YjcwYjIzLWZmM2QtNDg2OS1hOTYxLTc3M2IxOTNmZDkzOSIsInVzYWdlRW5kcG9pbnQiOiJodHRwczovL3Byb3h5LWV2ZW50LmNrZWRpdG9yLmNvbSIsImRpc3RyaWJ1dGlvbkNoYW5uZWwiOlsiY2xvdWQiLCJkcnVwYWwiXSwiZmVhdHVyZXMiOlsiRFJVUCJdLCJ2YyI6ImI5NDM2ZmFjIn0.ALcOwp9_11_tI3i7KGeosKtbsWle7S_kbanKTrB4L-A00mtjFfvg3_ty8PDzXmLE4wu3kd-T-7j0VqL5TusWcQ';

const editorConfig = {
	toolbar: {
		items: ['bold', 'italic', 'underline', '|', 'link', '|', 'alignment', '|', 'bulletedList', 'numberedList'],
		shouldNotGroupWhenFull: false
	},
	plugins: [Alignment, AutoLink, Autosave, Bold, Essentials, ImageEditing, ImageUtils, Italic, Link, List, Paragraph, Underline],
	initialData: '',
	licenseKey: LICENSE_KEY,
	link: {
		addTargetToExternalLinks: true,
		defaultProtocol: 'https://',
		decorators: {
			toggleDownloadable: {
				mode: 'manual',
				label: 'Downloadable',
				attributes: {
					download: 'file'
				}
			}
		}
	},
	placeholder: 'Type or paste your content here!'
};

ClassicEditor.create(document.querySelector('#editor'), editorConfig);
