#include <stdio.h>

int main(int argc, char const *argv[]) {
    int i;

    for (i = 2; i < 32; i++) {
        printf("raven%03d\n", i);
    }

    return 0;
}
