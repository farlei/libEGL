#pragma once
// libegl4: Cabecalho para configuracao automatica das bibliotecas no Visual Studio


#if _MSC_VER >= 1600
   // this is Visual C++ 2010
	#include "SDL.h"
	#include "SDL_image.h"
	#include "SDL_ttf.h"
	#include "SDL_mixer.h"	

	//SDL_gfx
	#include "SDL_gfxPrimitives.h" //Graphic Primitives 
	#include "SDL_rotozoom.h" //Rotozoomer 
	#include "SDL_framerate.h" //Framerate control 
	#include "SDL_imageFilter.h" //MMX image filters 
	//#include "SDL_gfxBlitFunc.h" //Custom Blit functions 

	#include "SDL_Collide.h"

	#ifndef _DEBUG
		#pragma message("RELEASE EGL4: added libs: sdl.lib sdlmain.lib sdl_image.lib (VS2010+)")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdlmain.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_image.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_ttf.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_gfx.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_mixer.lib")
		#pragma comment(lib, "..//..//libEGL//lib//NET2010//libegl.lib")
	#else
		#pragma message("DEBUG EGL4: added libs: sdl.lib sdlmain.lib sdl_image.lib (VS2010+)")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdlmain.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_image.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_ttf.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_gfx.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_mixer.lib")
		#pragma comment(lib, "..//..//libEGL//lib//NET2010//libegl_d.lib")
	#endif	
#else
#if _MSC_VER >= 1500
   // this is Visual C++ 2008
	#include "SDL.h"
	#include "SDL_image.h"
	#include "SDL_ttf.h"
	#include "SDL_mixer.h"	

	//SDL_gfx
	#include "SDL_gfxPrimitives.h" //Graphic Primitives 
	#include "SDL_rotozoom.h" //Rotozoomer 
	#include "SDL_framerate.h" //Framerate control 
	#include "SDL_imageFilter.h" //MMX image filters 
	//#include "SDL_gfxBlitFunc.h" //Custom Blit functions 

	#include "SDL_Collide.h"

	#ifndef _DEBUG
		#pragma message("RELEASE EGL4: added libs: sdl.lib sdlmain.lib sdl_image.lib (VS2008)")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdlmain.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_image.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_ttf.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_gfx.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_mixer.lib")
		#pragma comment(lib, "..//..//libEGL//lib//NET2008//libegl.lib")
	#else
		#pragma message("DEBUG EGL4: added libs: sdl.lib sdlmain.lib sdl_image.lib (VS2008)")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdlmain.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_image.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_ttf.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_gfx.lib")
		#pragma comment(lib, "..//..//libEGL//SDL//lib//sdl_mixer.lib")
		#pragma comment(lib, "..//..//libEGL//lib//NET2008//libegl_d.lib")
	#endif	
#else
	#pragma message("ERRO: Versão do Visual Studio sem suporte automático! ")
#endif 
#endif