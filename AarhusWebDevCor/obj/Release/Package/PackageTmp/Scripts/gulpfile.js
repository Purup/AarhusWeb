var gulp = require('gulp');
var sass = require('gulp-sass');
gulp.task('sass', function () {
    return gulp.src('css/styles.scss')
   .pipe(sass()) // Converts Sass to CSS with gulp-sass
   .pipe(gulp.dest('css/'))
});