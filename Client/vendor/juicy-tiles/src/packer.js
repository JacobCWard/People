/**
 * Packer
 * bin-packing algorithm
 */
(function( scope ){
"use strict";

/**
 * [Packer description]
 * @param {Object} [props] packer properties
 * @param {Number} [props.width=0] packer width (in px)
 * @param {Number} [props.height=0] packer height (in px)
 * @param {Number} [props.gap=0] gap between items (in px)
 * @param {String} [props.direction="rightDown"] packing direction `"rightDown"|"downRight"`
 * @param {Number} [props.x=0] x offset for all items (or position of package itself)
 * @param {Number} [props.y=0] y offset for all items (or position of package itself)
 * @TODO write tests for `#gap` (tomalec)
 */
function Packer( props /*width, height, direction*/ ){
  for ( var prop in props ) {
    this[ prop ] = props[ prop ];
  }
  this.reset();
}
Packer.prototype.width = 0;
Packer.prototype.height = 0;
Packer.prototype.gap = 0;
Packer.prototype.direction = "rightDown";

/**
 * Reset all free slots in packer.
 */
Packer.prototype.reset = function() {
  this.slots = [];
  var initialSlot = new Rectangle({
    x: this.x || 0,
    y: this.y || 0,
    width: this.width,
    height: this.height
  });

  this.slots.push( initialSlot );

  // set sorter
  this.sorter = sorters[ this.direction ] || sorters.rightDown;
};

/**
 * Find a slot and place rectanle there
 * @param {Rectanlge} rectangle  to add
 * @returns {Packer} packer itself
 */
Packer.prototype.add = function( rectangle ) {
  // trim too big rectangles
    if(rectangle.width > this.width){
      rectangle.width = this.width;
    }
    if(rectangle.height > this.height){
      rectangle.height = this.height;
    }

  for ( var si=0, len = this.slots.length; si < len; si++ ) {
    var slot = this.slots[si];
	//CHANGEME
    if ( slot.canFit( rectangle ) ) {
      this.placeAt( rectangle, slot );
      break;
    }
  }
  return this;
};
/** 
 * Place element at specyfic slot
 * @param  {Rectangle} rectangle  [description]
 * @param  {Rectangle | Object} slot slot to place, or at least object with position
 * @param  {Number} slot.x
 * @param  {Number} slot.y
 * @return {Rectangle}       placed rectangle
 * @IDEA implement version for placing at given slot (not point), to prevent iterating in #placed
 */
Packer.prototype.placeAt = function( rectangle, slot ) {
  // place rectangle in slot
  rectangle.x = slot.x;
  rectangle.y = slot.y;

  this.placed( rectangle );
  return rectangle;
};


/**
 * Update all free slots, for given new `rectangle`.
 * @param  {Rectangle} rectangle being added
 */
Packer.prototype.placed = function( rectangle ) {
  if(this.gap){
    rectangle.width += this.gap;
    rectangle.height += this.gap;
  }
  // update slots
  var revisedSlots = [];
  for ( var i=0, len = this.slots.length; i < len; i++ ) {
    var slot = this.slots[i];
    var newSlots = slot.getSlotsAround( rectangle );
    if ( newSlots ) { 
      // slot intersects with rectangle, add all slots around
      revisedSlots.push.apply( revisedSlots, newSlots );
    } else { 
      // this slot does not intersect with one to add
      revisedSlots.push( slot );
    }
  }
  if(this.gap){
    rectangle.width -= this.gap;
    rectangle.height -= this.gap;
  }

  this.slots = revisedSlots;

  Packer.cleanRedundant( this.slots );

  this.slots.sort( this.sorter );
};

/**
 * Remove redundant rectangle from array of rectangles
 * @param {Array<Rectangle>} rectangles array to clean
 * @returns {Array<Rectangle>} cleaned array
**/
Packer.cleanRedundant = function( rectangles ) {
  for ( var rectNo=0, len = rectangles.length; rectNo < len; rectNo++ ) {
    var currentRect = rectangles[rectNo];
    // skip over this rectangle if it was already removed
    if ( !currentRect ) {
      continue;
    }
    // clone rectangles we're testing
    var compareRects = rectangles.slice(0);
    // do not compare with self
    compareRects.splice( rectNo, 1 );
    // compare currentRect with others
    var removedCount = 0;
    for ( var compareNo=0, cLen = compareRects.length; compareNo < cLen; compareNo++ ) {
      var compareRect = compareRects[compareNo];
      var afterCurrentRect = rectNo > compareNo ? 0 : 1;
      if ( currentRect.contains( compareRect ) ) {
        // if currentRect contains another,
        // remove that rectangle from test collection
        rectangles.splice( compareNo + afterCurrentRect - removedCount, 1 );
        removedCount++;
      }
    }
  }

  return rectangles;
};

var sorters = {
  // align in horizontal layers RTL
  rightDown: function( a, b ) {
    return a.y - b.y || a.x - b.x;
  },
  // align in vertical layers UTB
  downRight: function( a, b ) {
    return a.x - b.x || a.y - b.y;
  }
};


// TODO: export
scope.Packer = Packer;

}(window));