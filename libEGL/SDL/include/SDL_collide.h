/*
    SDL_Collide:  A 2D collision detection library for use with SDL
    
    MIT License
    Copyright 2005-2006 SDL_collide Team
    http://sdl-collide.sourceforge.net
    All rights reserved.
    
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
    
    Amir Taaki
    genjix@crystalspace3d.org
    
    Rob Loach
    http://robloach.net
*/

/* A simple library for collision detection using SDL */

#ifndef _SDL_COLLIDE_h
#define _SDL_COLLIDE_h

/* Set up for C function definitions, even when using C++ */
#ifdef __cplusplus
//extern "C" {
#endif

#include <SDL.h>

/**
 * SDL surface test if offset (u,v) is a transparent pixel
 */
int SDL_CollideTransparentPixel(SDL_Surface *surface , int u , int v);

/**
 * Pixel-perfect collision between two surfaces.
 *
 * @param SA the first surface to check
 * @param Ax1 the top-left X-coordinate of surface @a SA
 * @param Ay1 the top-left Y-coordinate of surface @a SA
 * @param SB the second surface to check
 * @param Bx1 the top-left X-coordinate of surface @a SB
 * @param By1 the top-left Y-coordinate of surface @a SB
 * @param skip how many pixels the looping test should skip.
 *             1 is a truly perfect pixel test, but a value
 *             of 4 should be fine.
 *             this parameter is here to speed up the test at
 *             the expense of accuracy.
 *
 * @return non-zero on collision, 0 otherwise
 */
int SDL_CollidePixel(SDL_Surface *as , int ax , int ay ,
                       SDL_Surface *bs , int bx , int by, int skip = 4);

/**
 * Bounding-box collision between two surfaces.
 *
 * @param SA the first surface to check
 * @param Ax1 the top-left X-coordinate of surface @a SA
 * @param Ay1 the top-left Y-coordinate of surface @a SA
 * @param SB the second surface to check
 * @param Bx1 the top-left X-coordinate of surface @a SB
 * @param By1 the top-left Y-coordinate of surface @a SB
 *
 * @return non-zero on collision, 0 otherwise
 */
int SDL_CollideBoundingBox(SDL_Surface *sa , int ax , int ay ,
                             SDL_Surface *sb , int bx , int by);

/**
 * Bounding-box collision between two rectangles.
 *
 * @param A the first rectangle to check
 * @param B the second rectangle to check
 *
 * @return non-zero on collision, 0 otherwise
 */
int SDL_CollideBoundingBox(SDL_Rect a , SDL_Rect b);

/**
 * tests whether 2 circles intersect
 *
 * circle1 : centre (x1,y1) with radius r1
 * circle2 : centre (x2,y2) with radius r2
 *
 * (allow distance between circles of offset)
 */
int SDL_CollideBoundingCircle(int x1 , int y1 , int r1 ,
                                int x2 , int y2 , int r2 , int offset);

/**
 * a circle intersection detection algorithm that will use
 * the position of the centre of the surface as the centre of
 * the circle and approximate the radius using the width and height
 * of the surface (for example a rect of 4x6 would have r = 2.5).
 */
int SDL_CollideBoundingCircle(SDL_Surface *a , int x1 , int y1 ,
                                SDL_Surface *b , int x2 , int y2 ,
                                int offset);


/* Ends C function definitions when using C++ */
#ifdef __cplusplus
//}
#endif

#endif /* _SDL_COLLIDE_h */
