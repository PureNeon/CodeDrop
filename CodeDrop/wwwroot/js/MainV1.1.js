const codeArea = document.getElementById("code-area");
const SaveButton = document.getElementById("save-button");
const ShareButton = document.getElementById("share-button");

function PageLoaded()
{
	if (window.location.href.toString() != window.location.origin.toString() + '/')
	{
		GetData();
	}
}

function NewButtonClick()
{
	let stateObj;
	history.replaceState(stateObj, "CodeDrop", window.location.origin);
	window.location.reload(true);
}

function SaveButtonClick()
{
	if (SaveButton.textContent != 'Saved')
	{
		SaveButton.textContent = 'Saved';
		SaveButton.style.backgroundColor = 'green'
	}
	else
	{
		SaveButton.textContent = 'Save'
		SaveButton.style.backgroundColor = '#212529';
	}
	PostData();
}

function ShareButtonClick()
{
	navigator.clipboard.writeText(document.URL);
	if (ShareButton.textContent != 'Copied') {
		ShareButton.textContent = 'Copied';
		ShareButton.style.backgroundColor = 'green'
	}
	else {
		ShareButton.textContent = 'Share'
		ShareButton.style.backgroundColor = '#212529';
	}
}

function GetData()
{
	let searchArg = window.location.href.toString().replace(window.location.origin.toString() + '/', "");


	fetch(window.location.origin.toString() + '/api/' + searchArg)
		.then(response => {
			if (!response.ok) {
				throw new Error('Сеть ответила с ошибкой: ' + response.status);
			}
			return response.json();
		})
		.then(data => codeArea.value = data.message)
		.catch(error => console.error('Ошибка Fetch:', error));
}

function PostData()
{
	let stateObj;
	try {
		fetch(document.URL, {
			method: "POST",
			body: JSON.stringify({ Message: codeArea.value }),
			headers: {
				"Content-type": "application/json; charset=UTF-8"
			}
		})
			.then((response) => response.json())
			.then((json) => {
				if (!document.URL.includes(json.message))
					history.replaceState(stateObj, "CodeDrop", document.URL + json.message);
			});
	} catch (error) {
		console.error('Ошибка:', error);
	}
}

PageLoaded();