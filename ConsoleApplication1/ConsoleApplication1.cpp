#include <stdio.h>
#include <thread>
#include <shared_mutex>
#include <Windows.h>

static std::shared_mutex s_mtx{};

void f() {
	s_mtx.unlock();
}

int main()
{
	using namespace std;

	s_mtx.lock();

	thread th(f);
	th.detach();

	s_mtx.lock();

	printf("Success");
}
