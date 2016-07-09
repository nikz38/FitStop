/// <binding ProjectOpened='watch' />
module.exports = function (grunt) {
    'use strict';

    grunt.initConfig({

        pkg: grunt.file.readJSON('package.json'),

        watch: {
            files: ['app/**/*.js'],
            tasks: ['jshint', 'concat', 'ngAnnotate', 'uglify'],
        },

        jshint: {
            all: {
                src: ['app/**/*.js'],
            },
        },

        concat: {

            dist: {
                src: ['app/**/*.js'],
                dest: 'dist/<%= pkg.name %>.js'
            }
        },

        ngAnnotate: {
            options: {
                singleQuotes: true,
            },
            app: {
                files: [
                    {
                        'dist/<%= pkg.name %>.js': ['dist/<%= pkg.name %>.js']
                    },
                ],
            }
        },

        uglify: {
            options: {
                banner: '/*! <%= pkg.name %> <%= pkg.version %> <%= grunt.template.today("dd-mm-yyyy") %> */\n'
            },
            dist: {
                files: {
                    'dist/<%= pkg.name %>.min.js': ['<%= concat.dist.dest %>']
                }
            }
        }

    });

    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-ng-annotate');
    grunt.loadNpmTasks('grunt-contrib-jshint');

    grunt.registerTask('default', ['jshint', 'concat', 'ngAnnotate', 'uglify']);
};