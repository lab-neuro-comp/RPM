#ifndef RESIZING_H
#define RESIZING_H 0

#include <stdio.h>

void resize_file(char *inlet) {
    char outlet[256];
    sprintf(outlet, "convert %s -resize 130x100 %s", inlet);
    system(outlet);
}

#endif
