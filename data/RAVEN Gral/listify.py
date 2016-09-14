import os

def get_options(directory):
    outlet = []

    for filename in os.listdir(directory):
        chops = filename.split('.')
        first_chop = chops[0]
        if first_chop not in outlet:
            outlet.append(first_chop)

    return outlet

def get_possible_answers(directory, options):
    possible_answers = []

    for option in options:
        answer = 6
        filename = '%s.8.png' % (option)
        if os.path.isfile(filename):
            answer = 8
        possible_answers.append(answer)

    return possible_answers

def main(arg):
    options = get_options(arg)
    possible_answers = get_possible_answers(arg, options)
    lines = map(lambda it: '%s %s 1' % (it[0], it[1]), zip(options, possible_answers))
    for line in lines:
        print(line)

if __name__ == '__main__':
    main('.')
