# WebView2 Template
## Project
 - Remove web cache files under the executable directory to AppData.
 - Embed WebView2 runtime and DLL as embedded resources for single-file deployment.
 - Example included:
	- invoke C# code from JavaScript.
## Testing
`http://localhost/test/test.html`
```html
<html>
  <head>
    <meta charset="UTF-8">
  </head>
  <body>
    <button id="btn">test</button>
    <script>
      const btn = document.getElementById("btn")
      btn.addEventListener('click', async function(){
        const hostObj = window.chrome.webview.hostObjects.example
        const message = encodeURIComponent("中文測試")
        const result = await hostObj.Test(1,2,message)
        alert(result)
      })
    </script>
  </body>
</html>
```