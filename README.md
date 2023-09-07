# findhash

Add user details in textbox to get the sha256 generated in the third column. 

Clicking encrypt will concatenate the textbox values and generate a AES256 encrypted text.

Decrypt will decrypt the value generated from clicking the encrypt button.

### Clone :
```shell
git clone https://github.com/bseptember/findme

cd findme
```

### Visual Studio:
```Run
Click the run button in visual studio after opening it.
```

### Executable
```bin

..\findhash\bin\Debug\net6.0-windows\findhash.exe

```

```Dockerfile

docker build -t findhash .

docker run -d --name myapp findhash

The output executable will be generated in /app/out
```

### Test:
#### Sample
![](files/sample.PNG)
