package main

// #include "resizing.h"
import "C"
import "io/ioutil"


// WARNING! THIS DO NOT WORK
func main() {
    files, _ := ioutil.ReadDir(".")

    for _, file := range files {
        name := file.Name()
        if valid(name) {
            C.resize_file(C.CString(name));
        }
    }
}
// WARNING! THIS DO NOT WORK

func valid(inlet string) bool {
	outlet := true
	ext := ".png"

	for i := 1; i <= 4 && outlet; i++ {
		if inlet[len(inlet)-i] != ext[len(ext)-i] {
			outlet = false
		}
	}

	return outlet
}
