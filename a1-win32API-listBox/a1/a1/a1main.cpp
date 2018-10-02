/*
*  FILE          : a1main.cpp
*  PROJECT       : PROG2120 - Windows and Mobile Programming: Assignment #1
*  PROGRAMMER    : Brendan Rushing
*  FIRST VERSION : 2018-09-11
*  DESCRIPTION   :
*	This assignment is an introduction to practice using the Win32 API.
*	Create two list boxes and a push button. 
*	Move strings from one list box to another.
*	The move button is highlighted when an item in the left list box is selected.
*/

//INCLUDE FILES
#include <windows.h>
#include <string>
//eo INCLUDE FILES



//CONSTANTS
#define IDC_MAIN_BUTTON		101		// Button identifier
#define IDC_MAIN_LISTBOX1	102		// Edit box identifier
#define IDC_MAIN_LISTBOX2	103
#define BUFFER_SIZE 256

const char name1[] = "John Smith";
const char name2[] = "Mark Ryan";
const char name3[] = "Jerry Hayes";
const char name4[] = "Anthony Hodgins";
const char name5[] = "Bart Simpson";

//WINDOW
#define WINDOW_X 200
#define WINDOW_Y 200
#define WINDOW_WIDTH 545
#define WINDOW_HEIGHT 425

//LIST BOX #1
#define LISTBOX_1_X 50
#define LISTBOX_1_Y 60
#define LISTBOX_1_WIDTH 125
#define LISTBOX_1_HEIGHT 200

//LIST BOX #2
#define LISTBOX_2_X 350
#define LISTBOX_2_Y 60
#define LISTBOX_2_WIDTH 125
#define LISTBOX_2_HEIGHT 200

//PUSH BUTTON
#define PB_X 220
#define PB_Y 150
#define PB_WIDTH 80
#define PB_HEIGHT 24
#define PB_NAME "Move"

#define DISABLE_BUTTON 0
#define ENABLE_BUTTON 1

//eo CONSTANTS

//OBJECTS
HWND hListBox;
HWND hListBox2;
HWND hWndButton;

LRESULT CALLBACK WinProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
//eo OBJECTS




/*
*	FUNCTION : WINAPI WinMain
*	DESCRIPTION : This is the call to start the Windows 32 API
*	
*	PARAMETERS : HINSTANCE hInst, HINSTANCE hPrevInst, LPSTR lpCmdLine, int nShowCmd
*	RETURNS : int
*/
int WINAPI WinMain(HINSTANCE hInst, HINSTANCE hPrevInst, LPSTR lpCmdLine, int nShowCmd)
{
	WNDCLASSEX wClass;
	ZeroMemory(&wClass, sizeof(WNDCLASSEX));
	wClass.cbClsExtra = NULL;
	wClass.cbSize = sizeof(WNDCLASSEX);
	wClass.cbWndExtra = NULL;
	wClass.hbrBackground = (HBRUSH)COLOR_WINDOW;
	wClass.hCursor = LoadCursor(NULL, IDC_ARROW);
	wClass.hIcon = NULL;
	wClass.hIconSm = NULL;
	wClass.hInstance = hInst;
	wClass.lpfnWndProc = (WNDPROC)WinProc;
	wClass.lpszClassName = "Window Class";
	wClass.lpszMenuName = NULL;
	wClass.style = CS_HREDRAW | CS_VREDRAW;

	if (!RegisterClassEx(&wClass))
	{
		int nResult = GetLastError();
		MessageBox(NULL,
			"Window class creation failed\r\n",
			"Window Class Failed",
			MB_ICONERROR);
	}

	HWND hWnd = CreateWindowEx(NULL,
		"Window Class",
		"Brendan Rushing: Week 1 Assignment",
		WS_OVERLAPPEDWINDOW,
		WINDOW_X,
		WINDOW_Y,
		WINDOW_WIDTH,
		WINDOW_HEIGHT,
		NULL,
		NULL,
		hInst,
		NULL);

	if (!hWnd)
	{
		int nResult = GetLastError();

		MessageBox(NULL,
			"Window creation failed\r\n",
			"Window Creation Failed",
			MB_ICONERROR);
	}

	ShowWindow(hWnd, nShowCmd);

	MSG msg;
	ZeroMemory(&msg, sizeof(MSG));

	while (GetMessage(&msg, NULL, 0, 0))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	return EXIT_SUCCESS;
}


/*
*	FUNCTION : CALLBACK WinProc
*	DESCRIPTION : This is the call to create WIN32 API objects and logic to handle event triggers
*
*	PARAMETERS : HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam
*	RETURNS : LRESULT
*/
LRESULT CALLBACK WinProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_CREATE:
	{
		// Create Listbox on lefthand side ------------------------------------------------------------------------------------------
		hListBox = CreateWindowEx(WS_EX_CLIENTEDGE,
			"LISTBOX",
			"",
			WS_CHILD | WS_VISIBLE | LBS_NOTIFY,
			LISTBOX_1_X,
			LISTBOX_1_Y,
			LISTBOX_1_WIDTH,
			LISTBOX_1_HEIGHT,
			hWnd,
			(HMENU)IDC_MAIN_LISTBOX1,
			GetModuleHandle(NULL),
			NULL);
		HGDIOBJ hfDefault = GetStockObject(DEFAULT_GUI_FONT);
		SendMessage(hListBox,
			WM_SETFONT,
			(WPARAM)hfDefault,
			MAKELPARAM(FALSE, 0));

		//Name #1 for listbox 1
		SendMessage(hListBox,
			LB_ADDSTRING,
			NULL,
			(LPARAM)name1);
		//Name #2 for listbox 1
		SendMessage(hListBox,
			LB_ADDSTRING,
			NULL,
			(LPARAM)name2);
		//Name #3 for listbox 1
		SendMessage(hListBox,
			LB_ADDSTRING,
			NULL,
			(LPARAM)name3);
		//Name #4 for listbox 1
		SendMessage(hListBox,
			LB_ADDSTRING,
			NULL,
			(LPARAM)name4);
		//Name #5 for listbox 1
		SendMessage(hListBox,
			LB_ADDSTRING,
			NULL,
			(LPARAM)name5);
		//-----------------------------------------------------------------------------------------------------------------------------

		// Create Listbox on righthand side -------------------------------------------------------------------------------------------
		hListBox2 = CreateWindowEx(WS_EX_CLIENTEDGE,
			"LISTBOX",
			"",
			WS_CHILD | WS_VISIBLE,
			LISTBOX_2_X,
			LISTBOX_2_Y,
			LISTBOX_2_WIDTH,
			LISTBOX_2_HEIGHT,
			hWnd,
			(HMENU)IDC_MAIN_LISTBOX2,
			GetModuleHandle(NULL),
			NULL);
		HGDIOBJ hfDefault2 = GetStockObject(DEFAULT_GUI_FONT);
		SendMessage(hListBox2,
			WM_SETFONT,
			(WPARAM)hfDefault,
			MAKELPARAM(FALSE, NULL));
		//-----------------------------------------------------------------------------------------------------------------------------


		// Create a Pushbutton --------------------------------------------------------------------------------------------------------
		hWndButton = CreateWindowEx(NULL,
			"BUTTON",
			PB_NAME,
			WS_TABSTOP | WS_VISIBLE | WS_DISABLED |
			WS_CHILD | BS_DEFPUSHBUTTON,
			PB_X,
			PB_Y,
			PB_WIDTH,
			PB_HEIGHT,
			hWnd,
			(HMENU)IDC_MAIN_BUTTON,
			GetModuleHandle(NULL),
			NULL);
		//-----------------------------------------------------------------------------------------------------------------------------

		SendMessage(hWndButton,
			WM_SETFONT,
			(WPARAM)hfDefault,
			MAKELPARAM(FALSE, NULL));
	}
	break;

	case WM_COMMAND:
		switch (LOWORD(wParam))
		{
		case IDC_MAIN_BUTTON:
		{
			// Check if an item in the list has been slected and get the index value of it
			char buffer[BUFFER_SIZE] = { 0 };
			int indexInList = (int)SendMessage(hListBox,
				LB_GETCURSEL,
				(WPARAM)0,
				(LPARAM)0);

			// Use the index value to read the text in list box item
			if (indexInList != LB_ERR){
				SendMessage(hListBox,
					LB_GETTEXT,
					indexInList,
					reinterpret_cast<LPARAM>(buffer));
			}

			//ADD HIGHLIGHTED ITEM TO LIST BOX #2
			SendMessage(hListBox2,LB_ADDSTRING,NULL,(LPARAM)buffer);

			//DELETE HIGHLIGHTED STRING FROM LIST BOX #1
			SendMessage(hListBox, LB_DELETESTRING, indexInList, (LPARAM)buffer);

			//DISABLE BUTTON AGAIN
			EnableWindow(hWndButton, DISABLE_BUTTON);
		}
		break;

		case IDC_MAIN_LISTBOX1:
			case LB_GETSEL:
				//If there is an item in listbox1 selected then enable the button
				int indexInList = (int)SendMessage(hListBox,
					LB_GETCURSEL,
					(WPARAM)0,
					(LPARAM)0);

				if (indexInList != LB_ERR) {

					//ENABLE BUTTON if item in list box#1 is selected
					EnableWindow(hWndButton, ENABLE_BUTTON);
				}
		
			break;
		}
		break;

	case WM_DESTROY:
	{
		PostQuitMessage(0);
		return EXIT_SUCCESS;
	}
	break;
	}

	return DefWindowProc(hWnd, msg, wParam, lParam);
}