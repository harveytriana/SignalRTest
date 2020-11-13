// The following sample code uses TypeScript and must be compiled to JavaScript
// before a browser can execute it.

// ---------------------------
// __awaiter
// ---------------------------
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};

// ---------------------------
// component
// ---------------------------
function StreamSample(hubUrl) {
    var _list = document.getElementById("messagesList");
    var _streamButton = document.getElementById("streamButton");
    var _uploadButton = document.getElementById("uploadButton");

    const _h = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();

    // read server stream
    _streamButton.addEventListener("click", (event) => __awaiter(this, void 0, void 0, function* () {
        _streamButton.setAttribute("disabled", "disabled");
        addItem("Reading server stream...");
        //_list.innerHTML = "";
        try {
            // Hub's method note:
            // CounterChannel: ChannelReader<T>
            // CounterEnumerable: IAsyncEnumerable<T>
            //
            _h.stream("CounterEnumerable", 10, 500)
                .subscribe({
                    next: (item) => {
                        addItem(item);
                    },
                    complete: () => {
                        addItem("Stream completed");
                        _streamButton.removeAttribute("disabled");
                    },
                    error: (err) => {
                        addItem(err);
                    },
                });
        }
        catch (e) {
            console.error(e.toString());
        }
        event.preventDefault();
    }));

    // runs async
    _uploadButton.addEventListener("click", (event) => __awaiter(this, void 0, void 0, function* () {
        _uploadButton.setAttribute("disabled", "disabled");
        addItem("Receiving from client...");

        const subject = new signalR.Subject();
        yield _h.send("UploadStreamChannel", subject);
        var iteration = 0;
        const intervalHandle = setInterval(() => {
            iteration++;
            addItem(iteration);
            subject.next(iteration.toString());
            if (iteration === 10) {
                clearInterval(intervalHandle);
                subject.complete();
                // 
                addItem('Subject -> Complete');
                _uploadButton.removeAttribute("disabled");
            }
        }, 500);
        event.preventDefault();
    }));
    // We need an async function in order to use await, but we want this code to run immediately,
    // so we use an "immediately-executed async function"
    (() => __awaiter(this, void 0, void 0, function* () {
        try {
            yield _h.start();
        }
        catch (e) {
            console.error(e.toString());
        }
    }))();

    function addItem(i) {
        var li = document.createElement("li");
        li.textContent = i;
        _list.appendChild(li);
    }
};