def find_p(limit=1000):
    perfect_numbers = [n for n in range(1, limit + 1) if n == sum([i for i in range(1, n) if n % i == 0])]
    print(f"1000以内的完数有: {perfect_numbers}")

find_p()
